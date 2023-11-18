using MathWars.Data;
using MathWars.Models;
using MathWars.Pages.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.Globalization;
using System.Threading.Tasks;

namespace MathWars.Pages.TaskPages;
[Authorize]
[BindProperties]
public class SolvingTaskModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public Tasks Task { get; set; }
    public Answers Answer { get; set; }
    private readonly UserManager<ApplicationUser> _userManager;

    public SolvingTaskModel(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
        
    }
    public void OnGet(int id)
    {
        Task = _db.Tasks.Find(id);
        // Ustawienie danych w sesji
        HttpContext.Session.SetString("TaskTitle", Task.Title);
        HttpContext.Session.SetString("TaskContent", Task.Content);
        HttpContext.Session.SetString("TaskCategory", Task.category);
        HttpContext.Session.SetInt32("TaskDifficultyLevel", Task.difficultyLevel);
    }

    public async Task<IActionResult> OnPost()
    {
        // Odczytanie danych z sesji
        var taskTitle = HttpContext.Session.GetString("TaskTitle");
        var taskContent = HttpContext.Session.GetString("TaskContent");
        var taskCategory = HttpContext.Session.GetString("TaskCategory");
        var taskDifficultyLevel = HttpContext.Session.GetInt32("TaskDifficultyLevel") ?? 0;

        Task.Title = taskTitle;
        Task.Content = taskContent;
        Task.Created = Task.Created;
        Task.category = taskCategory;
        Task.difficultyLevel = taskDifficultyLevel;

        ModelState.Clear();
		if (Answer.Answer == Task.Answer)
        {
            // Get the currently logged-in user
            var user = await _userManager.GetUserAsync(User);

            var answ = new Answers()
            {
                UserId = user.Id,
                User = user,
                Task = Task,
                TaskId = Task.Id,
                Answer = Answer.Answer,
            };

            Task.Answers.Add(answ);
           
            await _db.Answers.AddAsync(answ);
            _db.Tasks.Update(Task);
            await _db.SaveChangesAsync();


            ModelState.AddModelError(string.Empty, "Correct answer : )");

			return Page();
		}
        else
        {
			ModelState.AddModelError(string.Empty, "Wrong answer :( ");
            return Page();
        }
    }
}
