using MathWars.Entities;
using MathWars.Helpers;
using MathWars.Models;

namespace MathWars.Interfaces
{
    public interface ITaskRepository
    {
        Task CreateTaskAsync(Tasks task);
        Task<Tasks> GetTaskWithCategoriesByIdAsync(int id);
        Task<Tasks> GetTaskWithCategoriesAndAnswersByIdAsync(int id);
        Task<TaskSolvingModel> GetTaskWithAnswersByIdAsync(int? id);
        Task<PaginatedList<TaskToSolveModel>> GetTasksToSolveAsync(
            TaskToSolveParams taskToSolveParams, string userId);
		void DeleteTask(Tasks task);
        void DelateRelationWithAnswers(Tasks task);
        void DelateRelationWithCategories(Tasks task);
    }
}
