using MathWars.Data;
using MathWars.Entities;
using MathWars.Helpers;
using MathWars.Interfaces;

namespace MathWars.Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly ApplicationDbContext _context;
		public UserRepository(ApplicationDbContext context)
        {
			_context = context;
        }

		public async Task<ApplicationUser> GetUserByIdAsync(string id)
		{
			return await _context.Users.FindAsync(id);
		}

		public void DeleteUser(ApplicationUser applicationUser)
		{
            _context.Users.Remove(applicationUser);
        }

        public PaginatedList<ApplicationUser> GetLeaderboardUsers(
            IList<ApplicationUser> usersInRole, int? pageIndex, int pageSize)
        {
            var usersQuery = usersInRole
                .OrderByDescending(u => u.Level)
                .ThenByDescending(u => u.Experience);

            int count = usersQuery.Count();
            var items = usersQuery.Skip(((pageIndex ?? 1) - 1) * pageSize).Take(pageSize).ToList();

            return new PaginatedList<ApplicationUser>(items, count, pageIndex ?? 1, pageSize);
        }
    }
}
