using MathWars.Data;
using MathWars.Migrations;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MathWars.Pages.TaskPages;
[Authorize(Roles = "taskManager, admin")]
[ValidateAntiForgeryToken]
[BindProperties]
public class CreateTaskModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IConfiguration _configuration;
    public Tasks Task { get; set; }

    public IFormFile? ImageFile { get; set; }
	public List<int> SelectedCategoryIds { get; set; }
	public IEnumerable<TasksCategory> categorys { get; set; }
    public IEnumerable<AnswerTypes> AnswersTypesList { get; set; }


    public CreateTaskModel(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
    {
        _db = db;
        _webHostEnvironment = webHostEnvironment;   
        _configuration = configuration;
    }
    public void OnGet()
    {
        SelectedCategoryIds = new List<int>();
        categorys = _db.TasksCategory;
        AnswersTypesList = _db.AnswerTypes;
	}

    public async Task<IActionResult> OnPost()
    {
        if (TaskValidation())
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                // generating new uniqe file name 
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;

                var appDirectory = _webHostEnvironment.WebRootPath;

                var imageDirectory = _configuration.GetSection("ImagesDirectorys").GetValue<string>("forTasks");
               
                // Save image on server
                var filePath = Path.Combine(imageDirectory, uniqueFileName);

                var physicalFilePath = appDirectory + filePath;

                using (var fileStream = new FileStream(physicalFilePath, FileMode.Create))
                {
                    ImageFile.CopyTo(fileStream);
                }
                Task.ImagePath = filePath;
            }

            

            if (SelectedCategoryIds != null)
			{
				foreach (var categoryId in SelectedCategoryIds)
				{
					Task.TasksAndCategories.Add(new TasksAndCategories
                    {
						TaskId = Task.Id,
						TaskCategoryId = categoryId
					});
				}
			}

            await _db.Tasks.AddAsync(Task);
            await _db.SaveChangesAsync();

            return RedirectToPage("ViewTasks");
        }
        return Page();
    }

    private bool TaskValidation()
    {
        bool result = true;
        if (Task.difficultyLevel == 0)
        {
            ModelState.AddModelError(string.Empty, "Difficulty level field cannot be empty");
            result = false;
        }
        if (string.IsNullOrEmpty(Task.Title))
        {
            ModelState.AddModelError(string.Empty, "Title field cannot be empty");
            result = false;
        }
        if (Task.Content == null)
        {
            ModelState.AddModelError(string.Empty, "Content field cannot be empty");
            result = false;
        }
        
        return result;
    }
}
