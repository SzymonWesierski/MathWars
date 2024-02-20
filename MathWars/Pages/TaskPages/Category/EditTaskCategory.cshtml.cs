using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MathWars.Pages.TaskPages.Category;
[Authorize(Roles = "admin,taskManager")]
[BindProperties]
public class EditTaskCategoryModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public TasksCategory Category { get; set; } = new TasksCategory();

    public EditTaskCategoryModel(ApplicationDbContext db)
    {
        _db = db;
    }
    public async Task<IActionResult> OnGet(int id)
    {
        Category = await _db.TasksCategory
            .FirstOrDefaultAsync(c => c.Id == id) ?? new TasksCategory();

        if(Category == null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (Category == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _db.TasksCategory.Update(Category);
            await _db.SaveChangesAsync();
            return RedirectToPage("ViewTasksCategory");
        }
        return Page();
    }
}
