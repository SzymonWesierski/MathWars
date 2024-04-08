using AutoMapper;
using AutoMapper.QueryableExtensions;
using MathWars.Data;
using MathWars.Entities;
using MathWars.Helpers;
using MathWars.Interfaces;
using MathWars.Models;
using Microsoft.EntityFrameworkCore;

namespace MathWars.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public TaskRepository(ApplicationDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task CreateTaskAsync(Tasks task)
        {
            await _context.Tasks.AddAsync(task);
        }

        public async Task<Tasks> GetTaskWithCategoriesByIdAsync(int id)
        {
            return await _context.Tasks
                .Include(t => t.TasksAndCategories)
                    .ThenInclude(tc => tc.TaskCategory)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
        public void DeleteTask(Tasks task)
        {
            _context.Tasks.Remove(task);
        }

        public async Task<Tasks> GetTaskWithCategoriesAndAnswersByIdAsync(int id)
        {
            return await _context.Tasks
               .Include(t => t.Category)
               .Include(a => a.AnswersToTask)
               .FirstOrDefaultAsync(t => t.Id == id);
        }

		public async Task<TaskSolvingModel> GetTaskWithAnswersByIdAsync(int? id)
		{
			var task = await _context.Tasks
			   .Include(a => a.AnswersToTask)
			   .FirstOrDefaultAsync(t => t.Id == id);

            return new TaskSolvingModel
            {
                Id = task.Id,
                Content = task.Content,
                ImageUrl = task.ImageUrl,
				DifficultyLevel = task.DifficultyLevel,
                NumberOfCorrectAnswers = task.NumberOfCorrectAnswers,
				AnswersToTask = task.AnswersToTask
            };
		}

        public async Task<PaginatedList<TaskToSolveModel>> GetTasksToSolveAsync(
            TaskToSolveParams taskToSolveParams, string userId)
        {
            var taskQuery = _context.Tasks
                .Include(t => t.Category)
                .AsQueryable();

            if (taskToSolveParams.OnlyNotSolved)
                taskQuery = taskQuery
                    .Include(t => t.Answers)
                    .Where(t => !t.Answers.Any(a => a.UserId == userId));

            if (taskToSolveParams.DifficultyLevel > 0)
				taskQuery = taskQuery
                    .Where(t => t.DifficultyLevel == taskToSolveParams.DifficultyLevel);

            if (taskToSolveParams.CategoryId > 0)
				taskQuery = taskQuery
                    .Where(t => t.Category.Any(t => t.Id == taskToSolveParams.CategoryId));

			return await PaginatedList<TaskToSolveModel>.CreateAsync(
				taskQuery.AsNoTracking().ProjectTo<TaskToSolveModel>(_mapper.ConfigurationProvider),
				taskToSolveParams.PageNumber,
				taskToSolveParams.PageSize);
		}

		public void DelateRelationWithAnswers(Tasks task)
        {
            _context.AnswersToTasks.RemoveRange(task.AnswersToTask);
        }

        public void DelateRelationWithCategories(Tasks task)
        {
            _context.TasksAndCategories.RemoveRange(task.TasksAndCategories);
        }
    }
}
