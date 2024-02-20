using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MathWars.Pages
{
    [Authorize]
	[BindProperties]
	public class IndexProfileModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
		private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public IndexProfileModel(ApplicationDbContext db, ILogger<IndexModel> logger, UserManager<ApplicationUser> userManager, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _logger = logger;
            _userManager = userManager;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        public ApplicationUser CurrentUser { get; set; }
		public PaginatedList<Answers> AnswersList { get; set; }
        [Required(ErrorMessage = "Jeżeli chcesz zmienić zdjęcie to musisz je dodać"), Display(Name = "Dodaj zdjęcie: ")]
        public IFormFile? ImageFile { get; set; }
        public string Uid {  get; set; }

        public async Task<IActionResult> OnGet(int? pageIndex, string uid) 
        {
            Uid = uid;
            CurrentUser = await _userManager.FindByIdAsync(uid) ?? new ApplicationUser();

            if (CurrentUser == null)
			{
				return NotFound();
			}

			int pageSize = _configuration.GetSection("NumberOfElementsInList").GetValue<int>("Profil");

			IQueryable<Answers> answerQuery = _db.Answers
				.Include(a => a.Task).Where(a => a.UserId == CurrentUser.Id).OrderByDescending(a => a.SubmissionDate);

            AnswersList = await PaginatedList<Answers>.CreateAsync(answerQuery, pageIndex ?? 1, pageSize);

            return Page();
		}

        public async Task<IActionResult> OnPostUploadImageAsync()
        {
            if (ModelState.IsValid)
            {
                CurrentUser = await _userManager.FindByIdAsync(CurrentUser.Id) ?? new ApplicationUser();

                if (ImageFile != null && CurrentUser.ProfileImagePath != _configuration.GetSection("ProfilePicture").GetValue<string>("defaultProfilePicture"))
                {
                    var pathToDeleteImage = _webHostEnvironment.WebRootPath + CurrentUser.ProfileImagePath;
                    if (System.IO.File.Exists(pathToDeleteImage))
                    {
                        System.IO.File.Delete(pathToDeleteImage);
                    }
                }

                // Handle image upload
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    // generating new uniqe file name 
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;

                    var appDirectory = _webHostEnvironment.WebRootPath;

                    var imageDirectory = _configuration.GetSection("ImagesDirectorys").GetValue<string>("profilePictures");

                    if (imageDirectory == null || uniqueFileName == null)
                    {
                        // TODO logger should handle that error
                        ModelState.AddModelError("ImageFile", "Błąd ścieżki");
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
                        CurrentUser.ProfileImagePath = filePath;
                    }

                    var result = await _userManager.UpdateAsync(CurrentUser);

                    if (result.Succeeded)
                    {
                        return RedirectToPage();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }

            return RedirectToPage();
        }
    }
}