using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MathWars.Pages.TaskPages
{
    public class CreateTaskModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Tasks Task { get; set; }

        public CreateTaskModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                await _db.Tasks.AddAsync(Task);
                await _db.SaveChangesAsync();
                return RedirectToPage("ViewTasks");
            }
            return Page();
        }
    }
}
