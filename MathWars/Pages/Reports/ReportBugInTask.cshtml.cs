using MathWars.Extensions;
using MathWars.Interfaces;
using MathWars.Models;
using MathWars.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MathWars.Pages.Reports;

[BindProperties]
[Authorize]
public class ReportBugInTaskModel : PageModel
{
    private readonly IUnitOfWork _uow;
    private readonly IPhotoService _photoService;

    public ReportBugInTaskModel(IUnitOfWork uow, IPhotoService photoService)
    {
        _uow = uow;
        _photoService = photoService;
	}

    public UsersReportsModel UserReport { get; set; } = new UsersReportsModel();
    public IFormFile ImageFile { get; set; }

    public IActionResult OnGet(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        UserReport.TaskId = id.Value;

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

            if (await _uow.Complete()) return RedirectToPage("/TaskPages/SolvingTask", new { id = UserReport.TaskId });
            return Page();
        }
        return Page();
    }
}
