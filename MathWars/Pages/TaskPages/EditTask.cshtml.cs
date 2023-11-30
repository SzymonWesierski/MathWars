using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MathWars.Pages.TaskPages;
[Authorize]
[BindProperties]
public class EditTaskModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public Tasks Task { get; set; }
    public IEnumerable<TasksCategory> categorys { get; set; }

    public EditTaskModel(ApplicationDbContext db)
    {
        _db = db;
    }
    public void OnGet(int id)
    {
        Task = _db.Tasks.Find(id);
        Task.Category = _db.TasksCategory.Find(Task.CategoryId);
        categorys = _db.TasksCategory;
    }

    public async Task<IActionResult> OnPost()
    {
        var id = Task.CategoryId;
        var category = _db.TasksCategory.Find(id);

        if (category == null)
        {
            categorys = _db.TasksCategory;
        }
        else
        {
            Task.Category = category;

            if (TaskValidation())
            {
                _db.Tasks.Update(Task);
                await _db.SaveChangesAsync();
                return RedirectToPage("ViewTasks");
            }
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
        if (Task.Category == null)
        {
            ModelState.AddModelError(string.Empty, "Category field cannot be empty");
            result = false;
        }
        return result;
    }
}
