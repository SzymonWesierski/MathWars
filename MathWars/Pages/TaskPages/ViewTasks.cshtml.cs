using Microsoft.AspNetCore.Mvc.RazorPages;
using MathWars.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MathWars.Helpers;
using MathWars.Entities;

namespace MathWars.Pages.TaskPages
{
    [Authorize(Policy = "RequireAdminOrManagerRole")]
    public class ViewTasksModel : PageModel
	{
		private readonly ApplicationDbContext _db;
		private readonly IConfiguration _configuration;
		public ViewTasksModel(ApplicationDbContext db, IConfiguration configuration)
		{
			_db = db;
			_configuration = configuration;
		}

		public PaginatedList<Tasks> Tasks { get; set; }

		public async Task OnGetAsync(int? pageIndex)
		{
			int pageSize = _configuration.GetSection("NumberOfElementsInList").GetValue<int>("Task");
			IQueryable<Tasks> tasksQuery = _db.Tasks
				.Include(t => t.TasksAndCategories)
					.ThenInclude(tc => tc.TaskCategory)
				//.Include(a => a.AnswerType)
				.OrderByDescending(t => t.Created);

			Tasks = await PaginatedList<Tasks>.CreateAsync(tasksQuery, pageIndex ?? 1, pageSize);
		}
	}
}
