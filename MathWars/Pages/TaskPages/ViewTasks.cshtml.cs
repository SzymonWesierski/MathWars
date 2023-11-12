using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;

namespace MathWars.Pages.TaskPages;
[Authorize]
public class addTasksModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ApplicationDbContext _db;
    public IEnumerable<Tasks> Tasks { get; set; }

    public addTasksModel(ApplicationDbContext db, ILogger<IndexModel> logger)
    {
        _db = db;
        _logger = logger;
    }

    public void OnGet()
    {
        Tasks = _db.Tasks;
    }
}