using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using MathWars.Helpers;
using MathWars.Entities;
using MathWars.Interfaces;
using MathWars.Models;

namespace MathWars.Pages.TaskPages.Category
{
	[Authorize(Policy = "RequireAdminOrManagerRole")]
	[BindProperties]
	public class ViewTasksCategoryModel : PageModel
	{
		private readonly IUnitOfWork _uow;
		private readonly IConfiguration _configuration;
		private readonly ILogger<ViewTasksCategoryModel> _logger;
		public ViewTasksCategoryModel(IUnitOfWork uow, IConfiguration configuration, ILogger<ViewTasksCategoryModel> logger)
		{
			_uow = uow;
			_configuration = configuration;
			_logger = logger;
		}

		public TaskCategoryModel Category { get; set; }
		public PaginatedList<TasksCategory> Categories { get; set; }

		public async Task OnGetAsync(int? pageIndex)
		{
			int pageSize = _configuration.GetSection("NumberOfElementsInList")
				.GetValue<int>("Categories");

			IQueryable<TasksCategory> categoryQuery = _uow.TaskCategoryRepository
				.GetTaskCategoriesQueryable();

			Categories = await PaginatedList<TasksCategory>
				.CreateAsync(categoryQuery, pageIndex ?? 1, pageSize);
		}

		public async Task<IActionResult> OnPostCreateCategory()
		{
			if (ModelState.IsValid)
			{
				var ct = new TasksCategory
				{
					CategoryName = Category.CategoryName
				};

				await _uow.TaskCategoryRepository.CreateTaskCategoryAsync(ct);

				if (await _uow.Complete()) return RedirectToPage("ViewTasksCategory");

				_logger.LogError("Problem after saving new task category");

				return BadRequest();
			}
			return RedirectToPage();
		}

		public async Task<IActionResult> OnPostEditCategory(int id, string newName)
		{
			var category = await _uow.TaskCategoryRepository.FindTaskCategoryByIdAsync(id);

			if (category == null)
			{
				_logger.LogError("Can't find category by given id");
				return NotFound();
			}

			category.CategoryName = newName;

			_uow.TaskCategoryRepository.UpdateCategoryNameAsync(category);

			if (await _uow.Complete()) return RedirectToPage();

			_logger.LogError("Problem with saving data about updated category");
			return BadRequest();
		}

		public async Task<IActionResult> OnPostDeleteTaskCategory(int id)
		{
			var category = await _uow.TaskCategoryRepository.FindTaskCategoryByIdAsync(id);

			if (category == null) {
				_logger.LogError("Can't find category by given id");
				return NotFound(); 
			}

			_uow.TaskCategoryRepository.DeleteTaskCategory(category);

			if(await _uow.Complete()) return RedirectToPage();

			_logger.LogError("Problem with saving data about deleted category");
			return BadRequest();
		}
	}

}
