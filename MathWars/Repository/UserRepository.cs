using AutoMapper;
using MathWars.Data;
using MathWars.Entities;
using MathWars.Helpers;
using MathWars.Interfaces;
using MathWars.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace MathWars.Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string uid)
		{
			return await _context.Users.FindAsync(uid);
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

        public async Task<UserStatsModel> GetUserStats(string uid)
        {
            return await _context.Users
                .Where(u => u.Id == uid)
                .ProjectTo<UserStatsModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();


        }
    }
}
