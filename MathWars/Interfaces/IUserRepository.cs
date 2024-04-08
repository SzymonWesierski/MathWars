using MathWars.Entities;
using MathWars.Helpers;

namespace MathWars.Interfaces
{
	public interface IUserRepository
	{
		Task<ApplicationUser> GetUserByIdAsync(string id);
		void DeleteUser(ApplicationUser applicationUser);
		PaginatedList<ApplicationUser> GetLeaderboardUsers(
			IList<ApplicationUser> usersInRole, int? pageIndex, int pageSize);
    }
}
