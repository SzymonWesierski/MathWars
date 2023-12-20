using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;

namespace MathWars.Pages.TaskPages.Category;
[Authorize]
public class ViewTasksCategoryModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public IEnumerable<TasksCategory> Categories { get; set; }

    public ViewTasksCategoryModel(ApplicationDbContext db)
    {
        _db = db;
        Categories = new List<TasksCategory>();
    }

    public void OnGet()
    {
        Categories = _db.TasksCategory;
    }

}