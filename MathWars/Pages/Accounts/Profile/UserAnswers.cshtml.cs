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

namespace MathWars.Pages
{
    [Authorize]
    public class UserAnswersModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationUser currentUser;
        public IEnumerable<Answers> answers { get; set; }

        public UserAnswersModel(ApplicationDbContext db, ILogger<IndexModel> logger, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task OnGet()
        {
            currentUser = await _userManager.GetUserAsync(User);
			answers = _db.Answers.Include(a => a.Task).Where(a => a.UserId == currentUser.Id).ToList();
		}
    }
}