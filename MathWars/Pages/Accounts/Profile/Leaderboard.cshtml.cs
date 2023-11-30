using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MathWars.Pages.Accounts.Profile
{
    public class LeaderboardModel : PageModel
    {
		private readonly ILogger<IndexModel> _logger;
		private readonly ApplicationDbContext _db;
        public IEnumerable<ApplicationUser> users { get; set; }

        public LeaderboardModel(ILogger<IndexModel> logger, ApplicationDbContext db) 
        {
            _db = db;
			_logger = logger;
		}
        public void OnGet()
        {
            users = _db.Users 
                .OrderByDescending(u => u.Level)
                .ThenByDescending(u => u.Experience)
                .ToList();
        }
    }
}
