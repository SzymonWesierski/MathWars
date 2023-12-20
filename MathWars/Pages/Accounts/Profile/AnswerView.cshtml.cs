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
        public Answers Answer { get; set; }
        public AnswerViewModel(ApplicationDbContext db) 
        {
            _db = db;
        }
        public void OnGet(int Id)
        {
            Answer = _db.Answers.Find(Id);
        }
    }
}
