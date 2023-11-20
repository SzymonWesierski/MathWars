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
        public string category = "duuupa";
        public string difficultyLevel = "2";
        public IEnumerable<Tasks> tasks { get; set; }  

        public IndexModel(ApplicationDbContext db, ILogger<IndexModel> logger)
        {
            _db = db;
            _logger = logger;
        }

        public void OnGet()
        {
            tasks = taskQueryResult(difficultyLevel, category);
        }

        public IActionResult OnPost()
        {
            category = Request.Form["category"].ToString();
            difficultyLevel = Request.Form["difficultyLevel"].ToString();
            tasks = taskQueryResult(difficultyLevel, category);
            return Page();
        }

        private IEnumerable<Tasks> taskQueryResult(string difficultyLevel, string category)
        {
            int.TryParse(difficultyLevel, out int difficulty);

            return _db.Tasks.Where(t => t.category == category && t.difficultyLevel == difficulty).ToList();
            
        }
    }
}