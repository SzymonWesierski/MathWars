using Microsoft.AspNetCore.Mvc.RazorPages;
using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace MathWars.Pages.TaskPages
{
	[Authorize(Roles = "taskManager, admin")]
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
				.Include(a => a.AnswerType);

			Tasks = await PaginatedList<Tasks>.CreateAsync(tasksQuery, pageIndex ?? 1, pageSize);
		}
	}
}
