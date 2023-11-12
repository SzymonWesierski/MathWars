using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MathWars.Pages.TaskPages;
[Authorize]
[BindProperties]
public class CreateTaskModel : PageModel
{
    private readonly ApplicationDbContext _db;
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
        if (TaskValidation())
        {
            await _db.Tasks.AddAsync(Task);
            await _db.SaveChangesAsync();
            return RedirectToPage("ViewTasks");
        }
        return Page();
    }

    private bool TaskValidation()
    {
        bool result = true;
        if (Task.difficultyLevel == 0)
        {
            ModelState.AddModelError(string.Empty, "Difficulty level field cannot be empty");
            result = false;
        }
        if (string.IsNullOrEmpty(Task.Title))
        {
            ModelState.AddModelError(string.Empty, "Title field cannot be empty");
            result = false;
        }
        if (Task.Content == null)
        {
            ModelState.AddModelError(string.Empty, "Content field cannot be empty");
            result = false;
        }
        if (string.IsNullOrEmpty(Task.category))
        {
            ModelState.AddModelError(string.Empty, "Category field cannot be empty");
            result = false;
        }
        return result;
    }
}
