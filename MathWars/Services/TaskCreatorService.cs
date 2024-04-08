using MathWars.Entities;
using MathWars.Helpers;
using MathWars.Interfaces;
using MathWars.Models;
using MathWars.Pages.TaskPages.Category;

namespace MathWars.Services
{
    public class TaskCreatorService : ITaskCreatorService
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _configuration;
        private readonly IPhotoService _photoService;
        private readonly ILogger<ViewTasksCategoryModel> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IImageOnServerService _imageOnServerService;
        private readonly ISessionService _sessionService;

        public TaskCreatorService(IUnitOfWork uow, IConfiguration configuration, IPhotoService photoService,
            ILogger<ViewTasksCategoryModel> logger, IWebHostEnvironment webHostEnvironment,
            IImageOnServerService imageOnServerService, ISessionService sessionService)
        {
            _uow = uow;
            _configuration = configuration;
            _photoService = photoService;
            _logger = logger;
            _sessionService = sessionService;
            _webHostEnvironment = webHostEnvironment;
            _imageOnServerService = imageOnServerService;
        }

        public async Task<bool> UpdateTaskAsync(string pageSessionName, 
            TaskAnswersAndDifficulty taskAnswersAndDifficulty)
        {
            int taskId = _sessionService.GetTaskIdFromSession(pageSessionName);

            if (taskId == 0) return false;

            var task = await _uow.TaskRepository.GetTaskWithCategoriesAndAnswersByIdAsync(taskId);

            int difficultyLevel = taskAnswersAndDifficulty.DifficultyLevel;
            List<AnswerToTaskModel> answersToTask = taskAnswersAndDifficulty.AnswersToTaskList;
            List<int> correctAnswersList = taskAnswersAndDifficulty.CorrectAnswers;

            string taskContent = _sessionService.GetTaskContentFromSession(pageSessionName);
            if (string.IsNullOrEmpty(taskContent)) return false;

            var categoriesIdsDict = _sessionService.GetCategoriesIdsFromSession(pageSessionName);
            if (!categoriesIdsDict.Any()) return false;


            foreach (var i in correctAnswersList)
            {
                answersToTask[i].IsCorrect = true;
            }

            task.NumberOfCorrectAnswers = correctAnswersList.Count;

            task.Content = taskContent;
            task.DifficultyLevel = difficultyLevel;

            string imageUrlSession = _sessionService.GetImagePathFromSession(pageSessionName);
            if (!string.IsNullOrEmpty(imageUrlSession))
            {
                // Loading image
                string imageUrl = _webHostEnvironment.WebRootPath + imageUrlSession;
                if (System.IO.File.Exists(imageUrl))
                {
                    var imageBytes = System.IO.File.ReadAllBytes(imageUrl);
                    var imageStream = new MemoryStream(imageBytes);
                    var imageFile = new FormFile(imageStream, 0, imageBytes.Length, "imageFile", imageUrl);

                    var result = await _photoService.AddPhotoAsync(imageFile, ImageDirectoriesCloudinary.Tasks);

                    if (result.Error == null)
                    {
                        task.ImageUrl = result.SecureUrl.AbsoluteUri;
                        task.PublicImageId = result.PublicId;
                    }
                }

            }

            task.AnswersToTask.Clear();
            foreach (var answerModel in answersToTask)
            {
                var answer = new AnswersToTask
                {
                    Content = answerModel.Content,
                    PhotoUrl = "",
                    IsCorrect = answerModel.IsCorrect,
                    TaskId = answerModel.TaskId,
                };

                task.AnswersToTask.Add(answer);
            }

            List<int> categoriesIds = new List<int>();

            foreach (var list in categoriesIdsDict.Values)
            {
                categoriesIds.AddRange(list);
            }

            task.Category.Clear();
            foreach (var id in categoriesIds)
            {
                var category = await _uow.TaskCategoryRepository.FindTaskCategoryByIdAsync(id);

                if (category == null)
                {
                    _logger.LogError("Can't find category by id");
                    return false;
                }

                task.Category.Add(category);
            }

            if (await _uow.Complete()) return true;

            return false;
        }

        public async Task<bool> CreateTaskAsync(TaskAnswersAndDifficulty taskAnswersAndDifficulty, string pageSessionName)
        {
            int difficultyLevel = taskAnswersAndDifficulty.DifficultyLevel;
            List<AnswerToTaskModel> answersToTask = taskAnswersAndDifficulty.AnswersToTaskList;
            List<int> correctAnswersList = taskAnswersAndDifficulty.CorrectAnswers;

            string taskContent = _sessionService.GetTaskContentFromSession(pageSessionName);
            if (string.IsNullOrEmpty(taskContent)) return false;

            var categoriesIdsDict = _sessionService.GetCategoriesIdsFromSession(pageSessionName);
            if (!categoriesIdsDict.Any()) return false;


            foreach (var i in correctAnswersList)
            {
                answersToTask[i].IsCorrect = true;
            }

            var task = new Tasks
            {
                Content = taskContent,
                DifficultyLevel = difficultyLevel,
                ImageUrl = string.Empty,
                PublicImageId = string.Empty,
                NumberOfCorrectAnswers = correctAnswersList.Count,
                Category = new List<TasksCategory>(),
                AnswersToTask = new List<AnswersToTask>()
            };

            string imageUrlSession = _sessionService.GetImagePathFromSession(pageSessionName);
            if (!string.IsNullOrEmpty(imageUrlSession))
            {
                // Loading image
                string imageUrl = _webHostEnvironment.WebRootPath + imageUrlSession;
                if (File.Exists(imageUrl))
                {
                    var imageBytes = File.ReadAllBytes(imageUrl);
                    var imageStream = new MemoryStream(imageBytes);
                    var imageFile = new FormFile(imageStream, 0, imageBytes.Length, "imageFile", imageUrl);

                    var result = await _photoService.AddPhotoAsync(imageFile, ImageDirectoriesCloudinary.Tasks);

                    if (result.Error == null)
                    {
                        task.ImageUrl = result.SecureUrl.AbsoluteUri;
                        task.PublicImageId = result.PublicId;
                    }
                }
            }

            foreach (var answerModel in answersToTask)
            {
                var answer = new AnswersToTask
                {
                    Content = answerModel.Content,
                    PhotoUrl = "",
                    IsCorrect = answerModel.IsCorrect,
                    TaskId = answerModel.TaskId,
                };

                task.AnswersToTask.Add(answer);
            }

            List<int> categoriesIds = new List<int>();

            foreach (var list in categoriesIdsDict.Values)
            {
                categoriesIds.AddRange(list);
            }

            foreach (var id in categoriesIds)
            {
                var category = await _uow.TaskCategoryRepository.FindTaskCategoryByIdAsync(id);

                if (category == null)
                {
                    _logger.LogError("Can't find category by id");
                    return false;
                }

                task.Category.Add(category);
            }

            await _uow.TaskRepository.CreateTaskAsync(task);

            if (await _uow.Complete()) return true;

            return false;
        }

        public async Task GetResorcesFromDatabaseAndLoadToSession(Tasks task, string pageSessionName, 
            CategoriesListAndCategoryModel categoriesAndCategory)
        {
            _sessionService.SetTaskIdInSession(task.Id, pageSessionName);

            if (categoriesAndCategory.Categories.Count > 0)
            {
                var categoriesDict = new Dictionary<int, List<int>>();

                for (int i = 1; i <= categoriesAndCategory.Categories.TotalPages; i++)
                {
                    var categorieslist = await GetCategoriesAsync(i);
                    categoriesDict.Add(i, new List<int>());

                    foreach (var category in task.Category)
                    {
                        if (categorieslist.Contains(category))
                        {
                            categoriesDict[i].Add(category.Id);
                        }
                    }
                }
                _sessionService.SetCategoryIdsDictInSession(categoriesDict, pageSessionName);
            }

            _sessionService.SetTaskContentInSession(task.Content, pageSessionName);

            _sessionService.SetImagePathInSession(task.ImageUrl, pageSessionName);

            _sessionService.SetDifficultyLevelInSession(task.DifficultyLevel, pageSessionName);

            _sessionService.SetNumberOfAnswersInSession(task.AnswersToTask.Count, pageSessionName);

            List<AnswerToTaskModel> answers = new List<AnswerToTaskModel>();
            List<int> correctAnswers = new List<int>();
            var index = 0;
            foreach (var answer in task.AnswersToTask)
            {
                var answerModel = new AnswerToTaskModel()
                {
                    id = answer.id,
                    Content = answer.Content,
                    IsCorrect = answer.IsCorrect,
                    TaskId = answer.TaskId
                };

                answers.Add(answerModel);

                if (answerModel.IsCorrect) correctAnswers.Add(index);

                index++;
            }
            _sessionService.SetAnswersToTasksInSession(answers, pageSessionName);
            _sessionService.SetCorrectAnswersInSession(correctAnswers, pageSessionName);
        }

        public void RemoveCreateTaskDataFromSession(string pageSessionName)
        {
            _sessionService.RemoveCategoryIdListFromSession(pageSessionName);
            _sessionService.RemoveTaskContentFromSession(pageSessionName);
            _sessionService.RemoveDifficultyLevelFromSession(pageSessionName);
            _sessionService.RemoveAnswersToTasksFromSession(pageSessionName);
            _sessionService.RemoveNumberOfAnswersFromSession(pageSessionName);
            _sessionService.RemoveCorrectAnswersFromSession(pageSessionName);
            _sessionService.RemoveImagePathFromSession(pageSessionName);
        }

        public async Task<CategoriesListAndCategoryModel> GetResourcesStep1(
            CategoriesListAndCategoryModel categoriesAndCategory, string pageSessionName)
        {
            categoriesAndCategory.Categories = await GetCategoriesAsync(categoriesAndCategory.NextPageIndex);

            categoriesAndCategory.SelectedCategoryIds = GetCategoriesIdsListForCurrentPage(pageSessionName, categoriesAndCategory.NextPageIndex);

            return categoriesAndCategory;
        }
        public TaskContentModel GetResourcesStep2(string pageSessionName)
        {
            var taskContent = new TaskContentModel
            {
                Content = _sessionService.GetTaskContentFromSession(pageSessionName)
            };
            return taskContent;
        }

        public TaskImageModel GetResourcesStep3(TaskImageModel taskImage, string pageSessionName)
        {
            taskImage = new TaskImageModel
            {
                ImagePath = _sessionService.GetImagePathFromSession(pageSessionName)
            };

            return taskImage;
        }

        public TaskAnswersAndDifficulty GetResourcesStep4(
            TaskAnswersAndDifficulty taskAnswersAndDifficulty,
            string pageSessionName)
        {
            taskAnswersAndDifficulty.DifficultyLevel = _sessionService.GetDifficultyFromSession(pageSessionName);

            taskAnswersAndDifficulty.AnswersToTaskList = _sessionService.GetAnswerToTaskFromSession(pageSessionName);

            taskAnswersAndDifficulty.NumberOfAnswers = _sessionService.GetNumberOfAnswersFromSession(pageSessionName);

            taskAnswersAndDifficulty.CorrectAnswers = _sessionService.GetCorrectAnswersFromSession(pageSessionName);

            taskAnswersAndDifficulty.AnswersToTaskList = AddRemoveAnswersAdjustList(
                taskAnswersAndDifficulty.AnswersToTaskList,
                taskAnswersAndDifficulty.AnswersToTaskList.Count,
                taskAnswersAndDifficulty.NumberOfAnswers);

            return taskAnswersAndDifficulty;
        }

        public void SetResourcesStep1(List<int> selectedCategoryIds,
            string pageSessionName, int currentPageIndex)
        {
            _sessionService.SetCategoryIdListInSession(
                            selectedCategoryIds,
                            _sessionService.GetCategoriesIdsFromSession(pageSessionName),
                            currentPageIndex,
                            pageSessionName);
        }
        public void SetResourcesStep2(string content, string pageSessionName)
        {
            _sessionService.SetTaskContentInSession(content, pageSessionName);
        }
        public void SetResourcesStep3(string pageSessionName, IFormFile image)
        {
            var imagePathFromSession = _sessionService.GetImagePathFromSession(pageSessionName);

            // If there is an image already then delete if new is uploaded
            if (!string.IsNullOrEmpty(imagePathFromSession) && image != null)
            {
                DeleteImageFromServer(imagePathFromSession);
            }

            if (image != null)
            {
                var filePath = _imageOnServerService
                    .SaveImage(ImageDirectoriesOnServer.Task, image);

                _sessionService.SetImagePathInSession(filePath, pageSessionName);
            }

        }
        public void SetResourcesStep4(string pageSessionName, TaskAnswersAndDifficulty taskAnswersAndDifficulty)
        {
            _sessionService.SetDifficultyLevelInSession(taskAnswersAndDifficulty.DifficultyLevel, pageSessionName);

            _sessionService.SetNumberOfAnswersInSession(taskAnswersAndDifficulty.NumberOfAnswers, pageSessionName);

            taskAnswersAndDifficulty.AnswersToTaskList = AddRemoveAnswersAdjustList(
               taskAnswersAndDifficulty.AnswersToTaskList,
               taskAnswersAndDifficulty.AnswersToTaskList.Count,
               taskAnswersAndDifficulty.NumberOfAnswers);

            _sessionService.SetAnswersToTasksInSession(taskAnswersAndDifficulty.AnswersToTaskList, pageSessionName);

            _sessionService.SetCorrectAnswersInSession(taskAnswersAndDifficulty.CorrectAnswers, pageSessionName);
        }

        public void DeleteImageFromServer(string imagePathFromSession)
        {
            if (!string.IsNullOrEmpty(imagePathFromSession))
            {
                _imageOnServerService.DeleteImage(imagePathFromSession);
            }
        }
        public async Task<PaginatedList<TasksCategory>> GetCategoriesAsync(int? pageIndex)
        {
            int pageSize = _configuration.GetSection("NumberOfElementsInList")
               .GetValue<int>("Categories");

            IQueryable<TasksCategory> categoryQuery = _uow.TaskCategoryRepository
                .GetTaskCategoriesQueryable();

            return await PaginatedList<TasksCategory>
                .CreateAsync(categoryQuery, pageIndex ?? 1, pageSize);
        }

        public List<int> GetCategoriesIdsListForCurrentPage(string pageSessionName, int nextPage)
        {
            var categoriesIdsDict = _sessionService.GetCategoriesIdsFromSession(pageSessionName);

            if (categoriesIdsDict.Count > 0 && categoriesIdsDict.TryGetValue(nextPage, out List<int> value))
            {
                return value;
            }

            return new List<int>();
        }

        private List<AnswerToTaskModel> AddRemoveAnswersAdjustList(
            List<AnswerToTaskModel> answersToTaskList,
            int numberOfAnswers,
            int numberOfPossibleAnswers)
        {
            while (numberOfAnswers < numberOfPossibleAnswers)
            {
                answersToTaskList.Add(new AnswerToTaskModel());
                numberOfAnswers = answersToTaskList.Count;
            }

            if (numberOfAnswers > numberOfPossibleAnswers)
            {
                answersToTaskList.RemoveRange(
                    numberOfPossibleAnswers, numberOfAnswers - numberOfPossibleAnswers);
            }
            return answersToTaskList;
        }
    }
}
