using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MathWars.Data;
using MathWars.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace MathWars.Pages
{
    [Authorize]
	[BindProperties]
	public class IndexProfileModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
		private readonly IConfiguration _configuration;

        public IndexProfileModel(ApplicationDbContext db, ILogger<IndexModel> logger, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _db = db;
            _logger = logger;
            _userManager = userManager;
            _configuration = configuration;
        }

		public ApplicationUser CurrentUser { get; set; }
		public PaginatedList<Answers> AnswersList { get; set; }

		public async Task<IActionResult> OnGet(int? pageIndex)
		{
			CurrentUser = await _userManager.GetUserAsync(User) ?? new ApplicationUser();

			if (CurrentUser == null)
			{
				return NotFound();
			}

			int pageSize = _configuration.GetSection("NumberOfElementsInList").GetValue<int>("Profil");

			IQueryable<Answers> answerQuery = _db.Answers
				.Include(a => a.Task).Where(a => a.UserId == CurrentUser.Id);

            AnswersList = await PaginatedList<Answers>.CreateAsync(answerQuery, pageIndex ?? 1, pageSize);

            return Page();
		}
	}
}