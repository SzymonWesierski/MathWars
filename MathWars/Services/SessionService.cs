using MathWars.Interfaces;
using MathWars.Models;
using Newtonsoft.Json;

namespace MathWars.Services
{
    public class SessionService : ISessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public void SetCategoryIdListInSession(List<int> categoryIdsList, Dictionary<int, 
            List<int>> categoriesIdsDict, int pageIndex, string pageSessionName)
        {
            if (categoriesIdsDict.ContainsKey(pageIndex))
            {
                categoriesIdsDict[pageIndex] = categoryIdsList;
            }
            else
            {
                categoriesIdsDict.Add(pageIndex, categoryIdsList);
            }

            _httpContextAccessor.HttpContext.Session.SetString($"{pageSessionName}_CategoriesIdsDict",
                JsonConvert.SerializeObject(categoriesIdsDict));
        }

        public void SetCategoryIdsDictInSession(Dictionary<int, List<int>> categoriesDict, string pageSessionName)
        {
            _httpContextAccessor.HttpContext.Session.SetString($"{pageSessionName}_CategoriesIdsDict",
                    JsonConvert.SerializeObject(categoriesDict));
        }

        public void SetTaskContentInSession(string taskContent, string pageSessionName)
        {
            _httpContextAccessor.HttpContext.Session.SetString($"{pageSessionName}_TaskContent",
                JsonConvert.SerializeObject(taskContent));
        }

        public void SetTaskIdInSession(int taskId, string pageSessionName)
        {
            _httpContextAccessor.HttpContext.Session.SetInt32($"{pageSessionName}_TaskId", taskId);
        }

        public void SetNumberOfAnswersInSession(int numberOfAnswers, string pageSessionName)
        {
            _httpContextAccessor.HttpContext.Session.SetInt32($"{pageSessionName}_NumberOfAnswers",
                numberOfAnswers);
        }

        public void SetAnswersToTasksInSession(List<AnswerToTaskModel> answersToTaskList, string pageSessionName)
        {
            _httpContextAccessor.HttpContext.Session.SetString($"{pageSessionName}_AnswersToTasks", JsonConvert
                .SerializeObject(answersToTaskList));
        }

        public void SetCorrectAnswersInSession(List<int> correctAnswers, string pageSessionName)
        {
            _httpContextAccessor.HttpContext.Session.SetString($"{pageSessionName}_CorrectAnswers", JsonConvert
                    .SerializeObject(correctAnswers));
        }

        public void SetDifficultyLevelInSession(int taskDifficulty, string pageSessionName)
        {
            _httpContextAccessor.HttpContext.Session.SetString($"{pageSessionName}_DifficultyLevel", JsonConvert
                    .SerializeObject(taskDifficulty));
        }

        public void SetImagePathInSession(string filePath, string pageSessionName)
        {
            _httpContextAccessor.HttpContext.Session.SetString($"{pageSessionName}_TaskImagePath", JsonConvert
                .SerializeObject(filePath));
        }

        public string GetTaskContentFromSession(string pageSessionName)
        {
            var taskContent = _httpContextAccessor.HttpContext.Session.GetString($"{pageSessionName}_TaskContent");

            if (!string.IsNullOrEmpty(taskContent))
            {
                var Content = JsonConvert
                    .DeserializeObject<string>(taskContent);

                return Content;
            }
            return string.Empty;
        }

        public string GetImagePathFromSession(string pageSessionName)
        {
            var imagePathFromSession = _httpContextAccessor.HttpContext.Session.GetString($"{pageSessionName}_TaskImagePath");

            if (!string.IsNullOrEmpty(imagePathFromSession))
            {
                var imagePath = JsonConvert
                    .DeserializeObject<string>(imagePathFromSession);

                return imagePath;
            }
            return string.Empty;
        }

        public int GetDifficultyFromSession(string pageSessionName)
        {
            var difficultyLevel = _httpContextAccessor.HttpContext.Session.GetString($"{pageSessionName}_DifficultyLevel");

            if (!string.IsNullOrEmpty(difficultyLevel))
            {
                var difficulty = JsonConvert
                    .DeserializeObject<int>(difficultyLevel);
                return difficulty;
            }
            return 1;
        }

        public List<AnswerToTaskModel> GetAnswerToTaskFromSession(string pageSessionName)
        {
            var answersToTasks = _httpContextAccessor.HttpContext.Session.GetString($"{pageSessionName}_AnswersToTasks");

            if (!string.IsNullOrEmpty(answersToTasks))
            {
                var answers = JsonConvert
                    .DeserializeObject<List<AnswerToTaskModel>>(answersToTasks);
                return answers;
            }
            return new List<AnswerToTaskModel>();
        }

        public int GetNumberOfAnswersFromSession(string pageSessionName)
        {
            return _httpContextAccessor.HttpContext.Session.GetInt32($"{pageSessionName}_NumberOfAnswers") ?? 2;
        }

        public int GetTaskIdFromSession(string pageSessionName)
        {
            var numberOfAnswers = _httpContextAccessor.HttpContext.Session.GetInt32($"{pageSessionName}_TaskId") ?? 0;

            return numberOfAnswers;
        }

        public List<int> GetCorrectAnswersFromSession(string pageSessionName)
        {
            var correctAnswers = _httpContextAccessor.HttpContext.Session.GetString($"{pageSessionName}_CorrectAnswers");

            if (!string.IsNullOrEmpty(correctAnswers))
            {
                return JsonConvert.DeserializeObject<List<int>>(correctAnswers);
            }
            return new List<int>();
        }
        public Dictionary<int, List<int>> GetCategoriesIdsFromSession(string pageSessionName)
        {
            var categoriesIdsFromSession = _httpContextAccessor.HttpContext.Session.GetString($"{pageSessionName}_CategoriesIdsDict");

            if (!string.IsNullOrEmpty(categoriesIdsFromSession))
            {
                var categoriesIds = JsonConvert
                    .DeserializeObject<Dictionary<int, List<int>>>(categoriesIdsFromSession);
                return categoriesIds;
            }

            return new Dictionary<int, List<int>>();
        }

        public void RemoveCategoryIdListFromSession(string pageSessionName)
        {
            _httpContextAccessor.HttpContext.Session.Remove($"{pageSessionName}_CategoriesIdsDict");
        }

        public void RemoveTaskContentFromSession(string pageSessionName)
        {
            _httpContextAccessor.HttpContext.Session.Remove($"{pageSessionName}_TaskContent");
        }

        public void RemoveTaskIdFromSession(string pageSessionName)
        {
            _httpContextAccessor.HttpContext.Session.Remove($"{pageSessionName}_TaskId");
        }

        public void RemoveNumberOfAnswersFromSession(string pageSessionName)
        {
            _httpContextAccessor.HttpContext.Session.Remove($"{pageSessionName}_NumberOfAnswers");
        }

        public void RemoveAnswersToTasksFromSession(string pageSessionName)
        {
            _httpContextAccessor.HttpContext.Session.Remove($"{pageSessionName}_AnswersToTasks");
        }

        public void RemoveCorrectAnswersFromSession(string pageSessionName)
        {
            _httpContextAccessor.HttpContext.Session.Remove($"{pageSessionName}_CorrectAnswers");
        }

        public void RemoveDifficultyLevelFromSession(string pageSessionName)
        {
            _httpContextAccessor.HttpContext.Session.Remove($"{pageSessionName}_DifficultyLevel");
        }

        public void RemoveImagePathFromSession(string pageSessionName)
        {
            _httpContextAccessor.HttpContext.Session.Remove($"{pageSessionName}_TaskImagePath");
        }

    }
}
