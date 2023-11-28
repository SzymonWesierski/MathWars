using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace MathWars.Pages.TaskPages;
[Authorize]
[BindProperties]
public class SolvingTaskModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public Tasks Task { get; set; }
    public Answers Answer { get; set; }
    private readonly UserManager<ApplicationUser> _userManager;
	private readonly IConfiguration _configuration;

	public SolvingTaskModel(ApplicationDbContext db, UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _db = db;
        _userManager = userManager;
		_configuration = configuration;
	}
    public void OnGet(int id)
    {
        Task = _db.Tasks.Find(id);
        // Setup data in session
        HttpContext.Session.SetString("TaskTitle", Task.Title);
        HttpContext.Session.SetString("TaskContent", Task.Content);
        HttpContext.Session.SetString("TaskCategory", Task.category);
        HttpContext.Session.SetInt32("TaskDifficultyLevel", Task.difficultyLevel);
    }

    public async Task<IActionResult> OnPost()
    {
        // Read data from session
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
            
            //User LVL and EXP
            user = GetHowManyExperienceReached(user);   
			user = GetHowManyLevelsReached(user);

            await _db.Answers.AddAsync(answ);
            _db.Tasks.Update(Task);
            _db.Users.Update(user);
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

    private ApplicationUser GetHowManyLevelsReached(ApplicationUser user)
    {        
		while (user.Experience >= user.ExpToReachNewLvl)
		{
			user.Experience -= user.ExpToReachNewLvl;
			user.Level += 1;
			user.ExpToReachNewLvl = user.Level * GetLevelMultiplier();//Exp to reach new level pattern
		}
        return user;
	}

	private ApplicationUser GetHowManyExperienceReached(ApplicationUser user)
	{
		user.Experience += Task.difficultyLevel * GetExperienceMultiplier();// exp points pattern

		return user;
	}

	private int GetExperienceMultiplier()
	{
		if (_configuration != null)
		{
			var expMultiplierSection = _configuration.GetSection("Experience&Level");
			if (expMultiplierSection.Exists())
			{
                return expMultiplierSection.GetValue<int>("experienceMultiplier");
			}
		}

		ModelState.AddModelError(string.Empty, "Couldn't find the configuration for 'experienceMultiplier' !!!");
		return 0;
	}

	private int GetLevelMultiplier()
	{
		if (_configuration != null)
		{
			var lvlMultiplierSection = _configuration.GetSection("Experience&Level");
			if (lvlMultiplierSection.Exists())
			{
				return lvlMultiplierSection.GetValue<int>("lvlMultiplier");
			}
		}

		ModelState.AddModelError(string.Empty, "Couldn't find the configuration for 'lvlMultiplier' !!!");
		return 0;
	}
}
