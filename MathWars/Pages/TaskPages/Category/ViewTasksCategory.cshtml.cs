using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;

namespace MathWars.Pages.TaskPages.Category;
[Authorize]
public class ViewTasksCategoryModel : PageModel
{
    private readonly ApplicationDbContext _db;
	private readonly IConfiguration _configuration;
	public ViewTasksCategoryModel(ApplicationDbContext db, IConfiguration configuration)
    {
        _db = db;
		_configuration = configuration;
	}

	public PaginatedList<TasksCategory> Categories { get; set; }

	public async Task OnGetAsync(int? pageIndex)
	{
		int pageSize = _configuration.GetSection("NumberOfElementsInList").GetValue<int>("Categories");

		IQueryable<TasksCategory> categoryQuery = _db.TasksCategory.OrderByDescending(t => t.Created);

		Categories = await PaginatedList<TasksCategory>.CreateAsync(categoryQuery, pageIndex ?? 1, pageSize);
	}

}