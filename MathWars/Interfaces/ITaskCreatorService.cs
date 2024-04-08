using MathWars.Entities;
using MathWars.Helpers;
using MathWars.Models;

namespace MathWars.Interfaces
{
    public interface ITaskCreatorService
    {
        Task<bool> CreateTaskAsync(TaskAnswersAndDifficulty taskAnswersAndDifficulty, string pageSessionName);
        void RemoveCreateTaskDataFromSession(string pageSessionName);
        Task<CategoriesListAndCategoryModel> GetResourcesStep1(CategoriesListAndCategoryModel categoriesAndCategory, string pageSessionName);
        TaskContentModel GetResourcesStep2(string pageSessionName);
        TaskImageModel GetResourcesStep3(TaskImageModel taskImage, string pageSessionName);
        TaskAnswersAndDifficulty GetResourcesStep4(TaskAnswersAndDifficulty taskAnswersAndDifficulty, string pageSessionName);
        List<int> GetCategoriesIdsListForCurrentPage(string pageSessionName, int nextPage);
        Task GetResorcesFromDatabaseAndLoadToSession(Tasks task, string pageSessionName, CategoriesListAndCategoryModel categoriesAndCategory);
        void SetResourcesStep1(List<int> selectedCategoryIds, string pageSessionName, int currentPageIndex);
        void SetResourcesStep2(string content, string pageSessionName);
        void SetResourcesStep3(string pageSessionName, IFormFile image);
        void SetResourcesStep4(string pageSessionName, TaskAnswersAndDifficulty taskAnswersAndDifficulty);
        void DeleteImageFromServer(string imagePathFromSession);
        Task<PaginatedList<TasksCategory>> GetCategoriesAsync(int? pageIndex);
        Task<bool> UpdateTaskAsync(string pageSessionName, TaskAnswersAndDifficulty taskAnswersAndDifficulty);
    }
}
