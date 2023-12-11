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
        public IEnumerable<Tasks> TasksList { get; set; } = Enumerable.Empty<Tasks>();
        

        public IndexModel(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        public void OnGet()
        {
            Categories = GetCategorys();

			Category = _db.TasksCategory
                .Find(RandomCategoryId()) ?? new TasksCategory();

            difficultyLevel = _configuration.GetSection("FiltrTaskIndexPage")
                .GetValue<string>("DefaultDifficultyLevel") ?? String.Empty;

            TasksList = taskQueryResult();
        }

        public IActionResult OnPost()
        {
            Categories = GetCategorys();

			Category = _db.TasksCategory
                .Find(Category.Id) ?? new TasksCategory();

			difficultyLevel = Request.Form["difficultyLevel"].ToString();

            TasksList = taskQueryResult();

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