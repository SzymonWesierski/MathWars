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
		private int categoryId;
		private readonly IConfiguration _configuration;
		public List<Tasks> TasksList { get; set; } = new List<Tasks>();

		// Dodane właściwości dla paginacji
		public int CurrentPage { get; set; } = 1;
		public int TotalPages { get; set; }
		private int ItemsPerPage { get; } = 1; 


		public IndexModel(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

		public IActionResult OnGet(int currentPage = 1, string difficulty = null)
		{
			Categories = GetCategorys();

			if(Categories == null) return NotFound();

			Category = Categories.FirstOrDefault() ?? new TasksCategory();

            categoryId = Category.Id;

			difficultyLevel = difficulty ?? _configuration.GetSection("FiltrTaskIndexPage").GetValue<string>("DefaultDifficultyLevel") ?? String.Empty;

			var allTasks = taskQueryResult().ToList();
			CurrentPage = currentPage;
			TotalPages = (int)Math.Ceiling((double)allTasks.Count / ItemsPerPage);
			TasksList = allTasks.Skip((CurrentPage - 1) * ItemsPerPage).Take(ItemsPerPage).ToList();

			return Page();
		}




		public IActionResult OnPost(int currentPage, string difficultyLevel)
		{
			this.difficultyLevel = difficultyLevel;

			Categories = GetCategorys();

			Category = _db.TasksCategory.Find(Category.Id) ?? new TasksCategory();

			var allTasks = taskQueryResult().ToList();

			TotalPages = (int)Math.Ceiling(allTasks.Count / (double)ItemsPerPage);

			TasksList = allTasks.Skip((currentPage - 1) * ItemsPerPage).Take(ItemsPerPage).ToList();

			CurrentPage = currentPage;

			if (!TasksList.Any() && currentPage != 1)
			{
				return RedirectToPage(new { currentPage = 1, difficulty = this.difficultyLevel });
			}

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

        private IEnumerable<TasksCategory> GetCategorys() 
        {
            return _db.TasksCategory;
		}
	}
}