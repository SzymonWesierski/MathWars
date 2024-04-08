using MathWars.Extensions;
using MathWars.Interfaces;
using MathWars.Models;
using MathWars.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MathWars.Pages.Reports;

[BindProperties]
[Authorize]
public class ReportBugOnWebsiteModel : PageModel
{
	private readonly IUnitOfWork _uow;
	private readonly IPhotoService _photoService;

	public ReportBugOnWebsiteModel(IUnitOfWork uow, IPhotoService photoService)
	{
		_uow = uow;
		_photoService = photoService;
	}

	public UsersReportsModel UserReport { get; set; }
	public IFormFile ImageFile { get; set; }

	public IActionResult OnGet()
    {
        return Page();
    }

	public async Task<IActionResult> OnPostAsync()
	{
		if (ModelState.IsValid)
		{
			UserReport.UserId = User.GetUserId();

			// Handle image upload
			if (ImageFile != null)
			{
				var result = await _photoService.AddPhotoAsync(ImageFile, ImageDirectoriesCloudinary.Reports);
				if (result.Error == null)
				{
					UserReport.ImageUrl = result.SecureUrl.AbsoluteUri;
					UserReport.PublicImageId = result.PublicId;
				}
			}

			await _uow.UserReportsRepository.AddReportAsync(UserReport);

            if (await _uow.Complete()) return RedirectToPage("/Index");
            return Page();
		}
		return Page();
	}
}
