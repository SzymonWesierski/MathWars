using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MathWars.Pages.Accounts.Profile
{
    [Authorize]
    [BindProperties]
    public class AnswerViewModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public AnswerViewModel(ApplicationDbContext db) 
        {
            _db = db;
        }

        public Answers Answer { get; set; }
        public string Uid { get; set; }


        public async Task<IActionResult> OnGetAsync(int id)
        {
            Answer = await _db.Answers.Include(a => a.Task)
                .FirstOrDefaultAsync(a => a.Id == id);

            if(Answer == null)
            {
                return NotFound();
            }

            Uid = Answer.UserId;

            return Page();  
        }
    }
}
