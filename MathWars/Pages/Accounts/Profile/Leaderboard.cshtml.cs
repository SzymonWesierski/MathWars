using MathWars.Entities;
using MathWars.Helpers;
using MathWars.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MathWars.Pages.Accounts.Profile
{
    [Authorize]
    public class LeaderboardModel : PageModel
    {
        private readonly IUnitOfWork _uow;
		private readonly ILogger<IndexModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public LeaderboardModel(IUnitOfWork uow, ILogger<IndexModel> logger, 
            UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _uow = uow;
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

            Users = _uow.UserRepository.GetLeaderboardUsers(usersInRole, pageIndex, pageSize);

        }



    }
}
