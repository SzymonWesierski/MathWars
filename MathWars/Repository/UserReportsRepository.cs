using AutoMapper;
using MathWars.Data;
using MathWars.Entities;
using MathWars.Interfaces;
using MathWars.Models;
using Microsoft.EntityFrameworkCore;
using MathWars.Helpers;

namespace MathWars.Repository
{
    public class UserReportsRepository : IUserReportsRepository
	{
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;
		public UserReportsRepository(ApplicationDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task AddReportAsync(UsersReportsModel usersReportsModel)
		{
			var usersReports = _mapper.Map<UsersReports>(usersReportsModel);
			await _context.UsersReports.AddAsync(usersReports);
		}

        public async Task<PaginatedList<UsersReports>> GetReportsAsync(int? pageIndex, int pageSize)
        {
            IQueryable<UsersReports> reportsQuery = _context.UsersReports.OrderByDescending(r => r.Created);

            return await PaginatedList<UsersReports>
				.CreateAsync(reportsQuery, pageIndex ?? 1, pageSize);
        }

        public async Task<UsersReports> GetReportByIdAsync(int id)
		{
			return await _context.UsersReports
			.FirstOrDefaultAsync(c => c.Id == id);
        }

		public void DeleteReport(UsersReports usersReports)
		{
			_context.UsersReports.Remove(usersReports);
		}
    }
}
