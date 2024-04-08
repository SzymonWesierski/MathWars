using MathWars.Entities;
using MathWars.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MathWars.Pages.TaskPages;

[Authorize(Policy = "RequireAdminOrManagerRole")]
[BindProperties]
public class DeleteTaskModel : PageModel
{
    private readonly IPhotoService _photoService;
    private readonly IUnitOfWork _uow;

    public DeleteTaskModel(IUnitOfWork uow, IPhotoService photoService)
    {
        _uow = uow;
        _photoService = photoService;
    }

    public Tasks Task {  get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        Task = await _uow.TaskRepository
            .GetTaskWithCategoriesByIdAsync(id);

        if (Task == null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (Task.Id > 0)
        {
            var taskFromDb = await _uow.TaskRepository
                .GetTaskWithCategoriesAndAnswersByIdAsync(Task.Id);

            if (taskFromDb != null)
            {
                _uow.TaskRepository.DelateRelationWithAnswers(taskFromDb);

                _uow.TaskRepository.DelateRelationWithCategories(taskFromDb);

                if (!string.IsNullOrEmpty(taskFromDb.PublicImageId))
                {
                    await _photoService.DeletePhotoAsync(taskFromDb.PublicImageId);
                }
                _uow.TaskRepository.DeleteTask(taskFromDb);

                if(await _uow.Complete()) return RedirectToPage("ViewTasks");
                return Page();
            }
            return Page();
        }
        return NotFound();
    }
}
