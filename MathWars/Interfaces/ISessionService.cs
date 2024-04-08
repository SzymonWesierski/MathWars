using MathWars.Models;

namespace MathWars.Interfaces
{
    public interface ISessionService
    {
        void SetCategoryIdListInSession(List<int> categoryIdsList, Dictionary<int,
            List<int>> categoriesIdsDict, int pageIndex, string pageSessionName);
        void SetTaskContentInSession(string taskContent, string pageSessionName);
        void SetTaskIdInSession(int taskId, string pageSessionName);
        void SetNumberOfAnswersInSession(int numberOfAnswers, string pageSessionName);
        void SetAnswersToTasksInSession(List<AnswerToTaskModel> answersToTaskList, string pageSessionName);
        void SetCorrectAnswersInSession(List<int> correctAnswers, string pageSessionName);
        void SetDifficultyLevelInSession(int taskDifficulty, string pageSessionName);
        void SetImagePathInSession(string filePath, string pageSessionName);
        void SetCategoryIdsDictInSession(Dictionary<int, List<int>> categoriesDict, string pageSessionName);
        string GetTaskContentFromSession(string pageSessionName);
        string GetImagePathFromSession(string pageSessionName);
        int GetDifficultyFromSession(string pageSessionName);
        List<AnswerToTaskModel> GetAnswerToTaskFromSession(string pageSessionName);
        int GetNumberOfAnswersFromSession(string pageSessionName);
        int GetTaskIdFromSession(string pageSessionName);
        List<int> GetCorrectAnswersFromSession(string pageSessionName);
        Dictionary<int, List<int>> GetCategoriesIdsFromSession(string pageSessionName);
        void RemoveCategoryIdListFromSession(string pageSessionName);
        void RemoveTaskContentFromSession(string pageSessionName);
        void RemoveTaskIdFromSession(string pageSessionName);
        void RemoveNumberOfAnswersFromSession(string pageSessionName);
        void RemoveAnswersToTasksFromSession(string pageSessionName);
        void RemoveCorrectAnswersFromSession(string pageSessionName);
        void RemoveDifficultyLevelFromSession(string pageSessionName);
        void RemoveImagePathFromSession(string pageSessionName);
    }
}
