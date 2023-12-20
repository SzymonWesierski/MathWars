using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MathWars.Pages.Accounts.Profile
{
    public class LeaderboardModel : PageModel
    {
		private readonly ILogger<IndexModel> _logger;
		private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public IEnumerable<ApplicationUser> Users { get; set; }

        public LeaderboardModel(ILogger<IndexModel> logger, ApplicationDbContext db, UserManager<ApplicationUser> userManager) 
        {
            _db = db;
			_logger = logger;
            _userManager = userManager;
        }
        public void OnGet()
        {
            var usersWithRole = _userManager.GetUsersInRoleAsync("user").Result;

            Users = usersWithRole
                .OrderByDescending(u => u.Level)
                .ThenByDescending(u => u.Experience)
                .ToList();
        }
    }
}
