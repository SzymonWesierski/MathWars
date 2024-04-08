using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using MathWars.Entities;
using MathWars.Interfaces;
using MathWars.Helpers;
using MathWars.Models;
using MathWars.Extensions;

namespace MathWars.Pages
{
	[Authorize]
	[BindProperties]
	public class IndexModel : PageModel
	{
		private readonly IUnitOfWork _uow;
		private readonly IConfigurationService _configurationService;

		public IndexModel(IUnitOfWork uow, IConfigurationService configurationService)
		{
			_uow = uow;
			_configurationService = configurationService;
		}

		public List<TasksCategory> Categories { get; set; }
		public PaginatedList<TaskToSolveModel> TasksToSolve { get; set; }
		public int ExpForSolveTask { get; set; }
		public bool IsTaskSolvedByUser { get; set; } = false;
		public int PageSize { get; } = 1;
		public TaskToSolveParams TaskToSolveParams { get; set; }

		public async Task OnGetAsync(
			int? pageIndex, int? difficulty, int? categoryId, bool onlyNotSolved)
		{
			await GetResources(pageIndex, difficulty, categoryId, onlyNotSolved);
		}

		public IActionResult OnPostAsync()
		{
			return RedirectToPage("Index", new
			{
				pageIndex = TaskToSolveParams.PageNumber,
				difficulty = TaskToSolveParams.DifficultyLevel,
				categoryId = TaskToSolveParams.CategoryId,
				onlyNotSolved = TaskToSolveParams.OnlyNotSolved
			});
		}
		private async Task<IActionResult> GetResources(int? pageIndex, int? difficulty, int? categoryId, bool onlyNotSolved)
		{
			Categories = _uow.TaskCategoryRepository.GetTaskCategories();
			if (Categories == null) return NotFound();
			Categories.Insert(0, new TasksCategory { Id = 0, CategoryName = "Wszystkie" });

			TaskToSolveParams = new TaskToSolveParams
			{
				DifficultyLevel = difficulty ?? 0,
				CategoryId = categoryId ?? 0,
				PageNumber = pageIndex ?? 1,
				PageSize = PageSize,
				OnlyNotSolved = onlyNotSolved,
			};

			var userId = User.GetUserId();

			TasksToSolve = await _uow.TaskRepository
				.GetTasksToSolveAsync(TaskToSolveParams, userId) ?? 
					new PaginatedList<TaskToSolveModel>();

			if(TasksToSolve.Count > 0)
			{
				var taskId = TasksToSolve.First().Id;

				if (NumberOfUsersAttemptsToSolveTask(taskId, userId) > 0) IsTaskSolvedByUser = true;

				ExpForSolveTask = Progression.GetExp(TasksToSolve.First().DifficultyLevel,
					_configurationService.GetExperienceMultiplier());
			}
			
			return Page();
		}

		private int NumberOfUsersAttemptsToSolveTask(int taskId, string userId)
		{
			return _uow.UserAnswersRepository
						.DidUserSolvedTask(taskId, userId).Count();
		}
	}
}