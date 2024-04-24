using MathWars.Data;
using MathWars.Entities;
using MathWars.Interfaces;

namespace MathWars.Repository
{
	public class TaskRatingRepository : ITaskRatingRepository
	{
		private readonly ApplicationDbContext _context;

		public TaskRatingRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public TaskRating GetRatingAsync(string uid, int taskId)
		{
			return _context.TaskRating
				.Where(r => r.UserId == uid && r.TaskId == taskId)
				.FirstOrDefault();
		}

        public void RemoveRatingByIdAsync(TaskRating taskRating)
        {
			_context.TaskRating.Remove(taskRating);
        }

        public async Task AddRatingAsync(TaskRating taskRating)
		{
			await _context.TaskRating.AddAsync(taskRating);
		}

		public bool HasUserGivenStar(string uid, int taskId)
		{
			if (_context.TaskRating.Where(r => r.UserId == uid && r.TaskId == taskId).Any())
			{
				return true;
			}
			return false;
		}

    }
}
