using MathWars.Entities;

namespace MathWars.Interfaces
{
	public interface ITaskRatingRepository
	{
		Task AddRatingAsync(TaskRating taskRating);
        TaskRating GetRatingAsync(string uid, int taskId);
        bool HasUserGivenStar(string uid, int taskId);
        void RemoveRatingByIdAsync(TaskRating taskRating);
    }
}
