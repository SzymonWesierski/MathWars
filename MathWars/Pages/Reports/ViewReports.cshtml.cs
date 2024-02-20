using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace MathWars.Pages.Reports;
[Authorize(Roles = "admin,taskManager")]
public class ViewReportsModel : PageModel
{
	private readonly ApplicationDbContext _db;
	private readonly IConfiguration _configuration;
	public ViewReportsModel(ApplicationDbContext db, IConfiguration configuration)
	{
		_db = db;
		_configuration = configuration;
	}

	public PaginatedList<UsersReports> Reports { get; set; }

	public async Task OnGetAsync(int? pageIndex)
	{
		int pageSize = _configuration.GetSection("NumberOfElementsInList").GetValue<int>("Reports");
		IQueryable<UsersReports> reportsQuery = _db.UsersReports.OrderByDescending(r => r.Created);

		Reports = await PaginatedList<UsersReports>.CreateAsync(reportsQuery, pageIndex ?? 1, pageSize);
	}
}