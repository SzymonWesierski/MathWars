using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MathWars.Pages.TaskPages;

[Authorize(Roles = "taskManager, admin")]
[BindProperties]
public class DeleteTaskModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public Tasks? Task { get; set; }

    public DeleteTaskModel(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
    {
        _db = db;
        _webHostEnvironment = webHostEnvironment;
    }

    public void OnGet(int id)
    {
        Task = _db.Tasks
            .Include(t => t.TasksAndCategories)
            .ThenInclude(tc => tc.TaskCategory)
            .Include(at => at.AnswerType)
            .FirstOrDefault(t => t.Id == id);
    }

    public async Task<IActionResult> OnPost()
    {
        var taskFromDb = await _db.Tasks
            .Include(t => t.TasksAndCategories)
            .FirstOrDefaultAsync(t => t.Id == Task.Id);

        if (taskFromDb != null)
        {
            // Remove associated records from the join table
            _db.TasksAndCategories.RemoveRange(taskFromDb.TasksAndCategories);

            //Remove Image related to Task
            var pathToTaskImage = _webHostEnvironment.WebRootPath + taskFromDb.ImagePath;
            if (!string.IsNullOrEmpty(pathToTaskImage) && System.IO.File.Exists(pathToTaskImage))
            {
                System.IO.File.Delete(pathToTaskImage);
            }

            // Remove the task
            _db.Tasks.Remove(taskFromDb);

            await _db.SaveChangesAsync();
            return RedirectToPage("ViewTasks");
        }

        return Page();

    }
}
