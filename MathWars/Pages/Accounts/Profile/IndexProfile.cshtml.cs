﻿using Microsoft.AspNetCore.Mvc;
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
    public class IndexProfileModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
		[BindProperty]
		public ApplicationUser currentUser { get; set; }
		[BindProperty]
		public IEnumerable<Answers> answers { get; set; }

        public IndexProfileModel(ApplicationDbContext db, ILogger<IndexModel> logger, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGet()
        {
            currentUser = await _userManager.GetUserAsync(User) ?? new ApplicationUser();

            if(currentUser == null)
            {
                return NotFound();
            }

			answers = _db.Answers
                .Include(a => a.Task).Where(a => a.UserId == currentUser.Id)
                .ToList();

            return Page();
		}
    }
}