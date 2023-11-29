using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MathWars.Pages.TaskPages;
[Authorize]
[BindProperties]
public class DeleteTaskCategoryModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public TasksCategory category { get; set; }

    public DeleteTaskCategoryModel(ApplicationDbContext db)
    {
        _db = db;
    }
    public void OnGet(int id)
    {
        category = _db.TasksCategory.Find(id);
    }

    public async Task<IActionResult> OnPost()
    {
        var categoryFromDb = _db.TasksCategory.Find(category.Id);
        if (categoryFromDb != null)
        {
            _db.TasksCategory.Remove(categoryFromDb);
            await _db.SaveChangesAsync();
            return RedirectToPage("ViewTasksCategory");
        }
        return Page();

    }
}
