using MathWars.Data;
using MathWars.Entities;
using MathWars.Interfaces;

namespace MathWars.Repository
{
    public class TaskCategoryRepository : ITaskCategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateTaskCategoryAsync(TasksCategory category)
        {
            await _context.TasksCategory.AddAsync(category);
        }

        public void DeleteTaskCategory(TasksCategory category)
        {
            _context.TasksCategory.Remove(category);
        }

        public async Task<TasksCategory> FindTaskCategoryByIdAsync(int id)
        {
            return await _context.TasksCategory.FindAsync(id);
        }

		public IQueryable<TasksCategory> GetTaskCategoriesQueryable()
		{
			return _context.TasksCategory.OrderByDescending(t => t.Created);
		}

		public List<TasksCategory> GetTaskCategories()
		{
			return _context.TasksCategory.OrderByDescending(t => t.Created).ToList();
		}


		public void UpdateCategoryNameAsync(TasksCategory category)
        {
            _context.TasksCategory.Update(category);
        }
	}
}
