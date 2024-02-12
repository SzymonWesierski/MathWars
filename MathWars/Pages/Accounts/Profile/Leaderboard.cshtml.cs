using MathWars.Data;
using MathWars.Models;
using MathWars.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace MathWars.Pages.Accounts.Profile
{
    public class LeaderboardModel : PageModel
    {
		private readonly ILogger<IndexModel> _logger;
		private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public LeaderboardModel(ILogger<IndexModel> logger, ApplicationDbContext db, UserManager<ApplicationUser> userManager, IConfiguration configuration) 
        {
            _db = db;
			_logger = logger;
            _userManager = userManager;
            _configuration = configuration;
        }

        public PaginatedList<ApplicationUser> Users { get; set; }

        public async Task OnGetAsync(int? pageIndex)
        {
            int pageSize = _configuration.GetSection("NumberOfElementsInList")
                .GetValue<int>("Leaderboard");

            IList<ApplicationUser> usersInRole = await _userManager.GetUsersInRoleAsync("user");
            var usersQuery = usersInRole
                .OrderByDescending(u => u.Level)
                .ThenByDescending(u => u.Experience);

            int count = usersQuery.Count();
            var items = usersQuery.Skip(((pageIndex ?? 1) - 1) * pageSize).Take(pageSize).ToList();

            Users = new PaginatedList<ApplicationUser>(items, count, pageIndex ?? 1, pageSize);
        }



    }
}
