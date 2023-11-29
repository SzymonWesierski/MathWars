using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MathWars.Pages.TaskPages;
[Authorize]
[BindProperties]
public class CreateTaskCategoryModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public TasksCategory category { get; set; }

    public CreateTaskCategoryModel(ApplicationDbContext db)
    {
        _db = db;
    }
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
        if (TaskCategoryValidation())
        {
            await _db.TasksCategory.AddAsync(category);
            await _db.SaveChangesAsync();
            return RedirectToPage("ViewTasksCategory");
        }
        return Page();
    }

    private bool TaskCategoryValidation()
    {
        bool result = true;
        // Add validation
        return result;
    }
}
