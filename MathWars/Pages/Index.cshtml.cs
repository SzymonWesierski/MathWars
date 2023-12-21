using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MathWars.Data;
using MathWars.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace MathWars.Pages
{
    [Authorize]
    [BindProperties]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public TasksCategory Category { get; set; } = new TasksCategory();
		public IEnumerable<TasksCategory>? Categories { get; set; }
		public string difficultyLevel = String.Empty;
        private readonly IConfiguration _configuration;
		public List<Tasks> TasksList { get; set; } = new List<Tasks>();

		// Dodane właściwości dla paginacji
		public int CurrentPage { get; set; } = 1;
		public int TotalPages { get; set; }
		private int ItemsPerPage { get; } = 1; // Możesz dostosować tę wartość


		public IndexModel(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

		public void OnGet(int currentPage = 1, string difficulty = null)
		{
			Categories = GetCategorys();
			Category = _db.TasksCategory.Find(RandomCategoryId()) ?? new TasksCategory();

			// Użyj przekazanej wartości 'difficulty' lub wartości domyślnej z konfiguracji, jeśli 'difficulty' jest null.
			difficultyLevel = difficulty ?? _configuration.GetSection("FiltrTaskIndexPage").GetValue<string>("DefaultDifficultyLevel") ?? String.Empty;

			var allTasks = taskQueryResult().ToList();
			CurrentPage = currentPage;
			TotalPages = (int)Math.Ceiling((double)allTasks.Count / ItemsPerPage);
			TasksList = allTasks.Skip((CurrentPage - 1) * ItemsPerPage).Take(ItemsPerPage).ToList();
		}


		public IActionResult OnPost(int currentPage, string difficultyLevel)
		{
			// Zapisz wartości z formularza do właściwości modelu
			this.difficultyLevel = difficultyLevel;

			// Wczytanie kategorii
			Categories = GetCategorys();

			// Wyszukanie wybranej kategorii
			Category = _db.TasksCategory.Find(Category.Id) ?? new TasksCategory();

			// Pobranie przefiltrowanej listy zadań
			var allTasks = taskQueryResult().ToList();

			// Obliczenie liczby stron na podstawie liczby przefiltrowanych zadań
			TotalPages = (int)Math.Ceiling(allTasks.Count / (double)ItemsPerPage);

			// Aplikacja paginacji na przefiltrowanej liście zadań
			TasksList = allTasks.Skip((currentPage - 1) * ItemsPerPage).Take(ItemsPerPage).ToList();

			// Aktualizacja bieżącej strony
			CurrentPage = currentPage;

			// Jeśli nie ma żadnych zadań na liście po filtracji, zresetuj bieżącą stronę do 1
			if (!TasksList.Any() && currentPage != 1)
			{
				// Przekieruj do pierwszej strony z zachowaniem obecnego poziomu trudności
				return RedirectToPage(new { currentPage = 1, difficulty = this.difficultyLevel });
			}


			// Zwróć stronę wraz z przefiltrowanymi i spaginowanymi danymi
			return Page();
		}




		private IEnumerable<Tasks> taskQueryResult()
        {
            if (Category != null)
            {
                int.TryParse(difficultyLevel, out int difficulty);

                var filteredTasks = _db.Tasks
                    .Where(t =>
                        t.TasksAndCategories.Any(tc => tc.TaskCategory.CategoryName == Category.CategoryName) &&
                        t.difficultyLevel == difficulty)
                    .ToList();

                return filteredTasks;
            }

            return Enumerable.Empty<Tasks>();
        }
        private int RandomCategoryId()
        {
            //TODO random not static
            var Id = 1;
            return Id;
        }

        private IEnumerable<TasksCategory> GetCategorys() 
        {
            return _db.TasksCategory;
		}
	}
}