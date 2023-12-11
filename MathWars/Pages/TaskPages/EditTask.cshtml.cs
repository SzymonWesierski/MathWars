using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace MathWars.Pages.TaskPages;
[Authorize(Roles = "taskManager, admin")]
[BindProperties]
public class EditTaskModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IConfiguration _configuration;
    public Tasks Task { get; set; } = new Tasks();
    public IFormFile? ImageFile { get; set; }
    public IEnumerable<TasksCategory> Categories { get; set; } = Enumerable.Empty<TasksCategory>();
    public IEnumerable<AnswerTypes> AnswerTypesList { get; set; } = Enumerable.Empty<AnswerTypes>();
    public List<int> SelectedCategoryIds { get; set; } = new List<int>();
    public AnswerTypes TaskAnswerTypes { get; set; } = new AnswerTypes();

    public EditTaskModel(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
    {
        _db = db;
        _webHostEnvironment = webHostEnvironment;
        _configuration = configuration;
    }
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Task = await _db.Tasks
            .Include(t => t.AnswerType)
            .Include(t => t.TasksAndCategories)
            .ThenInclude(tc => tc.TaskCategory)
            .FirstOrDefaultAsync(m => m.Id == id) ?? new Tasks();

        if (Task == null)
        {
            return NotFound();
        }

        Categories = await _db.TasksCategory.ToListAsync();
        AnswerTypesList = await _db.AnswerTypes.ToListAsync();

        

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {

        if (!ModelState.IsValid)
        {
            Categories = await _db.TasksCategory.ToListAsync();
            AnswerTypesList = await _db.AnswerTypes.ToListAsync();

            var taskFromDb = await _db.Tasks
            .Include(t => t.AnswerType)
            .Include(t => t.TasksAndCategories)
            .ThenInclude(tc => tc.TaskCategory)
            .FirstOrDefaultAsync(m => m.Id == Task.Id);

            if (taskFromDb == null)
            {
                return NotFound();
            }

            Task = taskFromDb;

            return Page();
        }

        // Load the existing task with its current relationships
        var existingTask = await _db.Tasks
            .Include(t => t.AnswerType)
            .Include(t => t.TasksAndCategories)
            .FirstOrDefaultAsync(t => t.Id == Task.Id);

        if (existingTask == null)
        {
            return NotFound();
        }

        // Update the task
        var pathToExistingTaskOldImage = existingTask.ImagePath;
        _db.Entry(existingTask).CurrentValues.SetValues(Task);

        // Get the existing category ids
        var existingCategoryIds = existingTask.TasksAndCategories.Select(tc => tc.TaskCategoryId).ToList();

        // Find the category ids to be removed
        var categoriesToRemove = existingCategoryIds.Except(SelectedCategoryIds).ToList();

        // Remove relationships that are no longer selected
        foreach (var categoryId in categoriesToRemove)
        {
            var relationshipToRemove = existingTask.TasksAndCategories.FirstOrDefault(tc => tc.TaskCategoryId == categoryId);
            if (relationshipToRemove != null)
            {
                existingTask.TasksAndCategories.Remove(relationshipToRemove);
            }
        }

        // Add new relationships
        var categoriesToAdd = SelectedCategoryIds.Except(existingCategoryIds).ToList();
        foreach (var categoryId in categoriesToAdd)
        {
            existingTask.TasksAndCategories.Add(new TasksAndCategories { TaskId = Task.Id, TaskCategoryId = categoryId });
        }

        // Handle image upload
        if (ImageFile != null && ImageFile.Length > 0)
        {
            // generating new uniqe file name 
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;

            var appDirectory = _webHostEnvironment.WebRootPath;

            var imageDirectory = _configuration.GetSection("ImagesDirectorys").GetValue<string>("forTasks");

            if (imageDirectory == null || uniqueFileName == null)
            {
                // TODO logger should handle that error
                ModelState.AddModelError("ImageFile", "B³¹d œcie¿ki");
                return Page();
            }
            else
            {
                // Save image on server
                var filePath = Path.Combine(imageDirectory, uniqueFileName);

                var physicalFilePath = appDirectory + filePath;

                using (var fileStream = new FileStream(physicalFilePath, FileMode.Create))
                {
                    ImageFile.CopyTo(fileStream);
                }
                existingTask.ImagePath = filePath;
            }
        }

        //Remove old Image related to Task
        if (!string.IsNullOrEmpty(pathToExistingTaskOldImage))
        {
            var pathToDeleteImage = _webHostEnvironment.WebRootPath + pathToExistingTaskOldImage;
            if (System.IO.File.Exists(pathToDeleteImage))
            {
                System.IO.File.Delete(pathToDeleteImage);
            }
        }
        
        // Save changes to the database
        await _db.SaveChangesAsync();

        return RedirectToPage("ViewTasks");

    }

}
