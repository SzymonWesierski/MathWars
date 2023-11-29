using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MathWars.Pages.TaskPages;
[Authorize]
[BindProperties]
public class EditTaskCategoryModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public TasksCategory category { get; set; }

    public EditTaskCategoryModel(ApplicationDbContext db)
    {
        _db = db;
    }
    public void OnGet(int id)
    {
        category = _db.TasksCategory.Find(id);
    }

    public async Task<IActionResult> OnPost()
    {
        if (TaskCategoryValidation())
        {
            _db.TasksCategory.Update(category);
            await _db.SaveChangesAsync();
            return RedirectToPage("ViewTasksCategory");
        }
        return Page();
    }

    private bool TaskCategoryValidation()
    {
        bool result = true;
        // Category validation
        return result;
    }
}
