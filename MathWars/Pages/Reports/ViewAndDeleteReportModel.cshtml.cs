using MathWars.Entities;
using MathWars.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MathWars.Pages.Reports;
[Authorize(Policy = "RequireAdminOrManagerRole")]
[BindProperties]
public class ViewAndDeleteReportModel : PageModel
{
    private readonly IUnitOfWork _uow;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IPhotoService _photoService;

    public ViewAndDeleteReportModel(IUnitOfWork uow, UserManager<ApplicationUser> userManager, IPhotoService photoService)
    {
        _uow = uow;
        _userManager = userManager;
        _photoService = photoService;
    }

    public UsersReports Report { get; set; }
    public string UserName { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        Report = await _uow.UserReportsRepository.GetReportByIdAsync(id);

        if (Report == null) return NotFound();

        var tempUser = await _userManager.FindByIdAsync(Report.UserId);

        if (tempUser == null) UserName = "unknown";
            
        UserName = tempUser.UserName;

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (Report == null) return NotFound();

        var reportFromDb = await _uow.UserReportsRepository.GetReportByIdAsync(Report.Id);

        if (reportFromDb != null)
        {
            //Delete image
			if (reportFromDb.ImageUrl != null)
			{
                await _photoService.DeletePhotoAsync(reportFromDb.PublicImageId);
			}

            _uow.UserReportsRepository.DeleteReport(reportFromDb);

            if(await _uow.Complete()) return RedirectToPage("ViewReports");
            return Page();
        }
        return Page();

    }
}
