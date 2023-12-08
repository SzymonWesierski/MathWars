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
    private readonly ILogger<IndexModel> _logger;
    private readonly ApplicationDbContext _db;
    public IEnumerable<Tasks> Tasks { get; set; }

    public ViewTasksModel(ApplicationDbContext db, ILogger<IndexModel> logger)
    {
        _db = db;
        _logger = logger;
    }

    public void OnGet()
    {
        Tasks = _db.Tasks
            .Include(t => t.TasksAndCategories)
                .ThenInclude(tc => tc.TaskCategory)
            .Include(a => a.AnswerType)
            .ToList();
    }
}