using MathWars.Entities;
using MathWars.Helpers;
using MathWars.Models;

namespace MathWars.Interfaces
{
    public interface IUserReportsRepository
	{
		Task AddReportAsync(UsersReportsModel usersReports);
        Task<UsersReports> GetReportByIdAsync(int id);
        Task<PaginatedList<UsersReports>> GetReportsAsync(int? pageIndex, int pageSize);
        void DeleteReport(UsersReports usersReports);
    }
}
