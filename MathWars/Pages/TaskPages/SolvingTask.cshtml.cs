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
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace MathWars.Pages.TaskPages;
[Authorize]
[BindProperties]
public class SolvingTaskModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public Tasks Task { get; set; } = new Tasks();
    public Answers Answer { get; set; } = new Answers();
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    public bool AnswerResult = false;
    public List<string> AnswersList { get; set; }


    public SolvingTaskModel(ApplicationDbContext db, UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _db = db;
        _userManager = userManager;
		_configuration = configuration;
	}
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Task = await _db.Tasks
            .Include(t => t.AnswerType)
            .FirstOrDefaultAsync(m => m.Id == id) ?? new Tasks();

        if (Task == null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        try
        {
            if (Task == null)
            {
                return NotFound();
            }

            int Id = Task.Id;

            if (Id == 0)
            {
                return NotFound();
            }

            Task = await _db.Tasks
                .Include(t => t.AnswerType)
                .FirstOrDefaultAsync(m => m.Id == Id) ?? new Tasks();

            if (Task == null)
            {
                return NotFound();
            }

            if (IsAnswerCorrect())
            {
                // Get the currently logged-in user
                var user = await _userManager.GetUserAsync(User) ?? new ApplicationUser();

                if (user == null)
                {
                    return NotFound();
                }

                var answ = new Answers()
                {
                    UserId = user.Id,
                    User = user,
                    Task = Task,
                    TaskId = Task.Id,
                    Answer = Task.Answer,
                };

                Task.Answers.Add(answ);


                if (!DidUserRespondToTask(user))
                {
                    // User LVL and EXP
                    user = GetHowManyExperienceReached(user);
                    user = GetHowManyLevelsReached(user);
                }

                await _db.Answers.AddAsync(answ);
                await _db.SaveChangesAsync();


                AnswerResult = true;
                
                return Page();
            }
            else
            {
                return Page();
            }
        }
        catch (Exception ex)
        {
            // Handle the exception or log it
            ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
            return Page();
        }
    }

    private bool DidUserRespondToTask(ApplicationUser user)
    {
        var result = _db.Answers.Where(t => t.TaskId == Task.Id && t.UserId == user.Id);

        if(result.Any())  return true; 

        return false;
    }


    private bool IsAnswerCorrect()
    {
        bool isWholeAnswerCorrect = true;

        //Prepering answers from form
        string answersFromForm = "";
        foreach (var answer in AnswersList)
        {
            answersFromForm += answer.Replace(" ", "").ToUpper() + ",";
        }
        answersFromForm = answersFromForm.Substring(0, answersFromForm.Length - 1);

        var answersFromFormList = answersFromForm.Split(",");
        var taskAnswersList = Task.Answer.Split(",");

        for (int i = 0; i < answersFromFormList.Length; i++)
        {
            bool invalid = true;
            for (int j = 0; j < taskAnswersList.Length; j++)
            {
                if (taskAnswersList[j] == answersFromFormList[i])
                {
                    taskAnswersList[j] = "";
                    invalid = false;
                    break;
                }
            }
            if (invalid)
            {
                ModelState.AddModelError($"AnswersList[{i}]", "Nie poprawna odpowiedŸ");
                isWholeAnswerCorrect = false;
            }
        }

        return isWholeAnswerCorrect;
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
