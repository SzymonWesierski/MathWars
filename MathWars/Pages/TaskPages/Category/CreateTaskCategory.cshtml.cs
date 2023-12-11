using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MathWars.Pages.TaskPages.Category;
[Authorize]
[BindProperties]
public class CreateTaskCategoryModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public TasksCategory Category { get; set; }

    public CreateTaskCategoryModel(ApplicationDbContext db)
    {
        _db = db;
        Category = new TasksCategory();
    }
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            await _db.TasksCategory.AddAsync(Category);
            await _db.SaveChangesAsync();
            return RedirectToPage("ViewTasksCategory");
        }
        return Page();
    }
}
