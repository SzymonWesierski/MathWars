using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace MathWars.Pages.TaskPages;
[Authorize(Roles = "taskManager, admin")]
public class ViewTasksModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public IEnumerable<Tasks> Tasks { get; set; } = Enumerable.Empty<Tasks>();

    public ViewTasksModel(ApplicationDbContext db)
    {
        _db = db;
    }

    public void OnGet()
    {
        Tasks = _db.Tasks
            .Include(t => t.TasksAndCategories)
                .ThenInclude(tc => tc.TaskCategory)
            .Include(a => a.AnswerType)
            .ToList() ?? Enumerable.Empty<Tasks>();
    }
}