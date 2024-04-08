using MathWars.Entities;
using MathWars.Helpers;
using MathWars.Interfaces;
using MathWars.Models;
using MathWars.Pages.TaskPages.Category;
using MathWars.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace MathWars.Pages.TaskPages.EditTask
{
    [Authorize(Policy = "RequireAdminOrManagerRole")]
    [ValidateAntiForgeryToken]
    [BindProperties]
    public class StartModel : PageModel
    {
        private readonly ILogger<ViewTasksCategoryModel> _logger;
        private readonly IUnitOfWork _uow;
        private readonly ISessionService _sessionService;
        private readonly ITaskCreatorService _taskCreatorService;

        public StartModel(ILogger<ViewTasksCategoryModel> logger, ISessionService sessionService,
            ITaskCreatorService taskCreatorService, IUnitOfWork uow)
        {
            _logger = logger;
            _sessionService = sessionService;
            CategoriesAndCategory = new CategoriesListAndCategoryModel();
            _taskCreatorService = taskCreatorService;
            _uow = uow;
        }

        public int Step { get; set; }
        public string PageSessionName = "EditTask";
		public TaskContentModel TaskContent { get; set; }
        public TaskImageModel TaskImage { get; set; }
        public TaskAnswersAndDifficulty TaskAnswersAndDifficulty { get; set; }
        public CategoriesListAndCategoryModel CategoriesAndCategory { get; set; }

        public async Task<IActionResult> OnGetAsync(int id, int? step)
        {
            if (id > 0)
            {
                Step = step ?? 1;

                await GetResources(Step);

                var task = await _uow.TaskRepository.GetTaskWithCategoriesAndAnswersByIdAsync(id);
                if (task == null) return NotFound();

                await _taskCreatorService.GetResorcesFromDatabaseAndLoadToSession(task, 
                        PageSessionName, CategoriesAndCategory);

                CategoriesAndCategory.SelectedCategoryIds = _taskCreatorService
                    .GetCategoriesIdsListForCurrentPage(PageSessionName,
                        CategoriesAndCategory.NextPageIndex);

                return Page();
            }
            return NotFound();
        }

		public async Task<IActionResult> OnPostAsync(int step)
		{
			Step = step;

			SetResources(++step);

			await GetResources(Step);

			return Page();
		}

		public async Task<IActionResult> OnPostCategoriesListAsync(int pageIndex)
		{
			Step = 1;

			SetResources(Step);

			CategoriesAndCategory.NextPageIndex = pageIndex;

			await GetResources(Step);

			return Page();
		}

		public async Task<IActionResult> OnPostStep1()
        {
            Step = 1;

			if (CategoriesAndCategory.SelectedCategoryIds.Count > 0 ||
					_sessionService.GetCategoriesIdsFromSession(PageSessionName).Values
						.SelectMany(v => v).Any())
			{
				SetResources(Step);

				await GetResources(++Step);

				return Page();
			}


			ModelState.AddModelError("SelectedCategoryIds", "Wybierz chocia¿ jedn¹ kategoriê");
            CategoriesAndCategory.Categories = await _taskCreatorService.
                GetCategoriesAsync(CategoriesAndCategory.CurrentPageIndex);

            return Page();
        }

        public async Task<IActionResult> OnPostStep2()
        {
            Step = 2;
            if (!String.IsNullOrEmpty(TaskContent.Content))
            {
                SetResources(Step);

                await GetResources(++Step);

                return Page();
            }
            ModelState.AddModelError("Content", "WprowadŸ jak¹œ treœæ zadania");

            return Page();
        }
        public async Task<IActionResult> OnPostStep3()
        {
            Step = 3;

            SetResources(Step);

            await GetResources(++Step);
            

            return Page();
        }
        public async Task<IActionResult> OnPostStep4()
        {
            Step = 4;

            SetResources(Step);

            if (isStep4Valid())
            {
                var isTaskUpdated = await _taskCreatorService.UpdateTaskAsync(PageSessionName, TaskAnswersAndDifficulty);

                if (!isTaskUpdated)
                {
                    return BadRequest();
                }

                var imagePathFromSession = _sessionService.GetImagePathFromSession(PageSessionName);
                if (!string.IsNullOrEmpty(imagePathFromSession))
                {
                    _taskCreatorService.DeleteImageFromServer(imagePathFromSession);
                }

                _taskCreatorService.RemoveCreateTaskDataFromSession(PageSessionName);

                return RedirectToPage("/TaskPages/ViewTasks");
            }

            await GetResources(Step);
            return Page();
        }

        public async Task<IActionResult> OnPostSetNumberOfAnswers()
        {
            if (TaskAnswersAndDifficulty.DifficultyLevel >= 1 ||
                TaskAnswersAndDifficulty.DifficultyLevel <= 6)
            {
                _sessionService.SetDifficultyLevelInSession(
                    TaskAnswersAndDifficulty.DifficultyLevel,
                    PageSessionName);
            }
            Step = 4;

            SetResources(Step);

            await GetResources(Step);
            return Page();
        }

		public async Task<IActionResult> OnPostCreateCategory()
		{
			if (!string.IsNullOrEmpty(CategoriesAndCategory.Category.CategoryName))
			{
				var ct = new TasksCategory
				{
					CategoryName = CategoriesAndCategory.Category.CategoryName
				};

				await _uow.TaskCategoryRepository.CreateTaskCategoryAsync(ct);

				if (await _uow.Complete()) return RedirectToPage("Start");

				_logger.LogError("Problem after saving new task category");

				return BadRequest();
			}
			return RedirectToPage();
		}

        private async Task GetResources(int step)
        {
            switch (step)
            {
                case 1:
                    CategoriesAndCategory = await _taskCreatorService
                        .GetResourcesStep1(CategoriesAndCategory, PageSessionName);
                    break;
                case 2:
                    TaskContent = _taskCreatorService.GetResourcesStep2(PageSessionName);

                    break;
                case 3:
                    TaskImage = _taskCreatorService.GetResourcesStep3(TaskImage, PageSessionName);
                    break;
                case 4:
                    TaskAnswersAndDifficulty = _taskCreatorService
                        .GetResourcesStep4(TaskAnswersAndDifficulty, PageSessionName);
                    break;

                default:
                    _logger.LogError("Step out of range in GetResources method");
                    throw new Exception("Step out of range in GetResources method");

            }
        }

        private void SetResources(int step)
        {
            switch (step)
            {
                case 1:
                    _taskCreatorService.SetResourcesStep1(CategoriesAndCategory.SelectedCategoryIds, PageSessionName,
                        CategoriesAndCategory.CurrentPageIndex);
                    break;

                case 2:
                    _taskCreatorService.SetResourcesStep2(TaskContent.Content, PageSessionName);
                    break;

                case 3:
                    _taskCreatorService.SetResourcesStep3(PageSessionName, TaskImage.Image);
                    break;

                case 4:
                    _taskCreatorService.SetResourcesStep4(PageSessionName, TaskAnswersAndDifficulty);
                    break;

                default:
                    _logger.LogError("Step out of range in SetResources method");
                    throw new Exception("Step out of range in SetResources method");
            }
        }

        private bool isStep4Valid()
        {
            var isValid = true;

            if (TaskAnswersAndDifficulty.DifficultyLevel < 1 ||
                TaskAnswersAndDifficulty.DifficultyLevel > 6)
            {
                ModelState.AddModelError("DifficultyLevel",
                    "Wybierz poziom trudnoœci");

                isValid = false;
            }

            if (TaskAnswersAndDifficulty.CorrectAnswers.Count < 1)
            {
                ModelState.AddModelError("CorrectAnswers",
                    "Zaznacz jedn¹ poprawn¹ odpowiedŸ");

                isValid = false;
            }

            if (TaskAnswersAndDifficulty.CorrectAnswers.Count ==
                TaskAnswersAndDifficulty.NumberOfAnswers)
            {
                ModelState.AddModelError("CorrectAnswers",
                    "Wszystkie odpowiedzi poprawnê, trochê bez sensu ;)");

                isValid = false;
            }

            for (int i = 0; i < TaskAnswersAndDifficulty.AnswersToTaskList.Count; i++)
            {
                if (String.IsNullOrEmpty(TaskAnswersAndDifficulty.AnswersToTaskList[i].Content) &&
                    TaskAnswersAndDifficulty.AnswersToTaskList[i].PhotoUrl == null)
                {
                    ModelState.AddModelError(String.Format("AnswersToTaskList[{0}]", i),
                        "WprowadŸ odpowiedŸ");

                    isValid = false;

                }
            }

            return isValid;
        }
    } 
}

