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
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _db;
        public TasksCategory? category { get; set; }
		public IEnumerable<TasksCategory>? categorys { get; set; }
		public string? difficultyLevel;
        private readonly IConfiguration _configuration;
        public IEnumerable<Tasks>? tasks { get; set; } = Enumerable.Empty<Tasks>();
        

        public IndexModel(ApplicationDbContext db, ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _db = db;
            _logger = logger;
            _configuration = configuration;
        }

        public void OnGet()
        {
            categorys = GetCategorys();

			category = _db.TasksCategory.Find(RandomCategoryId());

            difficultyLevel = _configuration.GetSection("FiltrTaskIndexPage")
                .GetValue<string>("DefaultDifficultyLevel");

            tasks = taskQueryResult();
        }

        public IActionResult OnPost()
        {
            categorys = GetCategorys();

			category = _db.TasksCategory.Find(category.Id);

			difficultyLevel = Request.Form["difficultyLevel"].ToString();

            tasks = taskQueryResult();

            return Page();
        }

        private IEnumerable<Tasks> taskQueryResult()
        {
            if (category != null)
            {
				int.TryParse(difficultyLevel, out int difficulty);
				return _db.Tasks.Where(t => t.Category.CategoryName == category.CategoryName && t.difficultyLevel == difficulty).ToList();
			}
			return null;
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