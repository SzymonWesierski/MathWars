using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using MathWars.Helpers;
using MathWars.Entities;
using MathWars.Interfaces;
using MathWars.Models;
using MathWars.Services;

namespace MathWars.Pages
{
    [Authorize]
    [ValidateAntiForgeryToken]
    [BindProperties]
	public class IndexProfileModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IUnitOfWork _uow;
        private readonly UserManager<ApplicationUser> _userManager;
		private readonly IConfiguration _configuration;
        private readonly IPhotoService _photoService;

        public IndexProfileModel(ILogger<IndexModel> logger, IUnitOfWork uow, 
            UserManager<ApplicationUser> userManager, IConfiguration configuration, 
            IPhotoService photoService)
        {
            _logger = logger;
            _uow = uow;
            _userManager = userManager;
            _configuration = configuration;
            _photoService = photoService;
        }

        public ApplicationUser CurrentUser { get; set; }
		public PaginatedList<UserProfileAnswerModel> AnswersList { get; set; }
        [Required(ErrorMessage = "Jeżeli chcesz zmienić zdjęcie to musisz je dodać"),
            Display(Name = "Dodaj zdjęcie: ")]
        public IFormFile ImageFile { get; set; }
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

            AnswersList = await _uow.UserAnswersRepository.GetUserProfileAnswersAsync(pageSize, pageIndex, uid);
            return Page();
		}

        public async Task<IActionResult> OnPostUploadImageAsync()
        {
            if (ModelState.IsValid)
            {
                CurrentUser = await _userManager.FindByIdAsync(CurrentUser.Id) ?? new ApplicationUser();

                var photoResult = await _photoService.AddPhotoAsync(ImageFile, ImageDirectoriesCloudinary.Profiles);

                if (photoResult.Error != null) return Page();

                CurrentUser.ProfileImageUrl = photoResult.SecureUrl.AbsoluteUri;
                CurrentUser.PublicProfileImageId = photoResult.PublicId;

                var result = await _userManager.UpdateAsync(CurrentUser);

                if (result.Succeeded)
                {
                    return RedirectToPage("IndexProfile", new {uid = CurrentUser.Id});
                }
                else
                {
                    return NotFound();
                }
            }

            return RedirectToPage();
        }
    }
}