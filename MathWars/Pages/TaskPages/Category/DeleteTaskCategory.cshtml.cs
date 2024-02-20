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
public class DeleteTaskCategoryModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public TasksCategory Category { get; set; }

    public DeleteTaskCategoryModel(ApplicationDbContext db)
    {
        _db = db;
        Category = new TasksCategory();
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

        var categoryFromDb = _db.TasksCategory.Find(Category.Id);
        if (categoryFromDb != null)
        {
            _db.TasksCategory.Remove(categoryFromDb);
            await _db.SaveChangesAsync();
            return RedirectToPage("ViewTasksCategory");
        }
        return Page();

    }
}
