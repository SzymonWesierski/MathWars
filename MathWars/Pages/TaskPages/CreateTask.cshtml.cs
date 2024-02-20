using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
    public Tasks Task { get; set; } = new Tasks();
    public IFormFile? ImageFile { get; set; }

    [Required(ErrorMessage = "Musisz wybraæ kategoriê")]
	public List<int> SelectedCategoryIds { get; set; } = new List<int>();

    public IEnumerable<TasksCategory> Categories { get; set; } = Enumerable.Empty<TasksCategory>();
    public IEnumerable<AnswerTypes> AnswersTypesList { get; set; } = Enumerable.Empty<AnswerTypes>();


    public CreateTaskModel(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
    {
        _db = db;
        _webHostEnvironment = webHostEnvironment;   
        _configuration = configuration;
    }
    public void OnGet()
    {
        Categories = _db.TasksCategory;
        AnswersTypesList = _db.AnswerTypes;
	}

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
        {
            // If the model state is not valid, return the page with validation errors.
            Categories = await _db.TasksCategory.ToListAsync();
            AnswersTypesList = await _db.AnswerTypes.ToListAsync();

            return Page();
        }

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
                Task.ImagePath = filePath;
            }
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
        var answerType = _db.AnswerTypes.FirstOrDefault(a => a.Id == Task.AnswerTypeId);

        if(answerType == null)  return BadRequest();

        var countAnswersForm = Task.Answer.Split(',').Select(s => s.Trim()).ToList().Count();

        if (countAnswersForm != answerType.HowManyCorrectAnswers)
        {
            ModelState.AddModelError("Task.Answer", $"Musisz podaæ {answerType.HowManyCorrectAnswers.ToString()} odpowedzi po przecinku");

			// If the model state is not valid, return the page with validation errors.
			Categories = await _db.TasksCategory.ToListAsync();
			AnswersTypesList = await _db.AnswerTypes.ToListAsync();

			return Page();
        }
        Task.Answer = Task.Answer.ToUpper().Replace(" ", "");

        await _db.Tasks.AddAsync(Task);
        await _db.SaveChangesAsync();

        return RedirectToPage("ViewTasks");

    }
}
