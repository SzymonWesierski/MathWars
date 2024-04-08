using MathWars.Entities;

namespace MathWars.Interfaces
{
    public interface ITaskCategoryRepository
    {
        Task CreateTaskCategoryAsync(TasksCategory category);

        void DeleteTaskCategory(TasksCategory category);

        Task<TasksCategory> FindTaskCategoryByIdAsync(int id);

		IQueryable<TasksCategory> GetTaskCategoriesQueryable();

        void UpdateCategoryNameAsync(TasksCategory category);
		List<TasksCategory> GetTaskCategories();
	}
}
