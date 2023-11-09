using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MathWars.Pages.TaskPages;
[BindProperties]
public class DeleteTaskModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public Tasks Task { get; set; }

    public DeleteTaskModel(ApplicationDbContext db)
    {
        _db = db;
    }
    public void OnGet(int id)
    {
        Task = _db.Tasks.Find(id);
    }

    public async Task<IActionResult> OnPost()
    {
        var taskFromDb = _db.Tasks.Find(Task.Id);
        if (taskFromDb != null)
        {
            _db.Tasks.Remove(taskFromDb);
            await _db.SaveChangesAsync();
            return RedirectToPage("ViewTasks");
        }
        return Page();
       
    }
}
