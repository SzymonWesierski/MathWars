using MathWars.Entities;
using MathWars.Helpers;
using MathWars.Models;

namespace MathWars.Interfaces
{
	public interface IUserAnswersRepository
	{
		Task<UserAnswer> GetUserAnswersAndTaskAsync(int id);
		IQueryable<UserAnswer> DidUserSolvedTask(int taskId, string userId);
		Task CreateUserAnswer(UserAnswer userAnswer);
		Task<PaginatedList<UserProfileAnswerModel>> GetUserProfileAnswersAsync(
			int pageSize, int? pageIndex, string userId);
    }
}
