using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MathWars.Entities;
using MathWars.Interfaces;
using MathWars.Models;
using MathWars.Extensions;
using MathWars.Helpers;

namespace MathWars.Pages.TaskPages;
[Authorize]
[BindProperties]
public class SolvingTaskModel : PageModel
{
    private readonly IUnitOfWork _uow;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<SolvingTaskModel> _logger;
    private readonly IConfigurationService _configurationService;

    public SolvingTaskModel(IUnitOfWork uow, UserManager<ApplicationUser> userManager, IConfigurationService configurationService)
    {
        _uow = uow;
        _userManager = userManager;
		_configurationService = configurationService;
	}

    public bool ShowModal { get; set; } = false;
    public bool IsSolved { get; set; } = false;
    public bool Error { get; set; } = false;
    public int NumberOfAttempts { get; set; } = 0;
    public TaskSolvingModel TaskSolvingModel { get; set; }
    public List<int> SelectedAnswersIds { get; set; }
    public UserStatsModel UserStatsModel { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        TaskSolvingModel = await _uow.TaskRepository.GetTaskWithAnswersByIdAsync(id);

        var userId = User.GetUserId();
        NumberOfAttempts = DidUserSolvedTaskYet(userId);

        if (TaskSolvingModel == null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        try
        {
            TaskSolvingModel = await _uow.TaskRepository
                .GetTaskWithAnswersByIdAsync(TaskSolvingModel.Id);

            if (SelectedAnswersIds.Count != TaskSolvingModel.NumberOfCorrectAnswers)
            {
                Error = true;
                if(TaskSolvingModel.NumberOfCorrectAnswers == 1)
                {
                    ModelState.AddModelError("TaskSolvingModel.AnswersToTask",
                        "Wybierz jedn¹ odpowiedŸ");
                }
                else
                {
                    ModelState.AddModelError("TaskSolvingModel.AnswersToTask",
                       $"Wybierz {TaskSolvingModel.NumberOfCorrectAnswers} odpowiedzi");
                }
                

                ShowModal = true;

                return Page();
            }

            var userId = User.GetUserId();
            var user = await _uow.UserRepository.GetUserByIdAsync(userId);

            if (user == null) return NotFound();

            if (IsAnswerCorrect())
            {
                NumberOfAttempts = DidUserSolvedTaskYet(userId);

                if (NumberOfAttempts == 0)
                {
                    var result = await SetUserExpAndLvlAsync(user);
                    if (!result)
                    {
                        Error = true;
						ModelState.AddModelError("TaskSolvingModel.AnswersToTask",
						"Coœ posz³o nie tak z zapisywaniem twojej odpowiedzi. " +
                        "Przepraszamy za niedogodnoœci. Je¿eli problem wyst¹pi ponownie " +
                        "mo¿esz go zg³oœci przez zak³adkê \"Zg³oœ b³¹d\"." +
                        "Postaramy siê naprawiæ ten b³¹d jak najszybciej siê da :)");
						return Page();
                    }
                }

                UserStatsModel.Exp = user.Experience;
                UserStatsModel.ExpToReachNewLvl = user.ExpToReachNewLvl;
                UserStatsModel.Level = user.Level;

                await CreateUserAnswer("", "", true, userId, TaskSolvingModel.Id);

                IsSolved = true;
                ShowModal = true;
                
                if (await _uow.Complete())
                {
                    return Page();
                }

                _logger.LogError("Problem with saving user answer");
                return Page();
            }
            else
            {
                await CreateUserAnswer("", "", false, userId, TaskSolvingModel.Id);

                ShowModal = true;
                if (await _uow.Complete())
                {
                    NumberOfAttempts = DidUserSolvedTaskYet(userId);
                    return Page();
                }
                _logger.LogError("Problem with saving user answer");
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
    
    private async Task CreateUserAnswer(string whiteBoardPhotoUrl, 
        string publicWhiteBoardPhotoId, bool isSolvedCorrect, 
        string userId, int taskId)
    {
        var answer = new UserAnswer
        {
            WhiteBoardPhotoUrl = whiteBoardPhotoUrl,
            PublicWhiteBoardPhotoId = publicWhiteBoardPhotoId,
            IsSolvedCorrect = isSolvedCorrect,
            UserId = userId,
            TaskId = taskId
        };

        await _uow.UserAnswersRepository.CreateUserAnswer(answer);
    }

    private bool IsAnswerCorrect()
    {
        foreach(var asnswer in TaskSolvingModel.AnswersToTask)
        {
            if (!asnswer.IsCorrect && SelectedAnswersIds.Contains(asnswer.id))
            {
                return false;
            }
        }
        return true;
    }

    private int DidUserSolvedTaskYet(string userId)
    {
        var result = _uow.UserAnswersRepository
            .DidUserSolvedTask(TaskSolvingModel.Id, userId);
        return result.Count();
    }

    private async Task<bool> SetUserExpAndLvlAsync(ApplicationUser user)
    {
		var expMultiplier = _configurationService.GetExperienceMultiplier();
		if (expMultiplier == 0)
		{
			ModelState.AddModelError(string.Empty,
				"Couldn't find the configuration for 'experienceMultiplier' !!!");
			_logger.LogError("Couldn't find the configuration for 'experienceMultiplier'");
            return false;
		}

		user = Progression.GetHowManyExperienceReached(user, 
            TaskSolvingModel.DifficultyLevel, expMultiplier);

		var lvlMultplier = _configurationService.GetLevelMultiplier();
		if (lvlMultplier == 0)
		{
			ModelState.AddModelError(string.Empty,
				"Couldn't find the configuration for 'lvlMultiplier' !!!");
			_logger.LogError("Couldn't find the configuration for 'lvlMultiplier' !!!");
            return false;
		}
		user = Progression.GetHowManyLevelsReached(user, lvlMultplier);

		var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded) return true;
        return false;
	}
}
