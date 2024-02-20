using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MathWars.Pages.Reports;
[Authorize]
[BindProperties]
public class ViewAndDeleteReportModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
	private readonly IWebHostEnvironment _webHostEnvironment;

	public ViewAndDeleteReportModel(ApplicationDbContext db,
        UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
    {
        _db = db;
        _userManager = userManager;
        _webHostEnvironment = webHostEnvironment;
    }

    public UsersReports Report { get; set; }
    public string UserName { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        Report = await _db.UsersReports
            .FirstOrDefaultAsync(c => c.Id == id) ?? new UsersReports();

        if (Report == null)
        {
            return NotFound();
        }

        var tempUser = await _userManager.FindByIdAsync(Report.UserId);

        if (tempUser == null)
        {
            UserName = "unknown";
        }
        else
        {
            UserName = tempUser.UserName;
        }

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (Report == null)
        {
            return NotFound();
        }

        var reportFromDb = _db.UsersReports.Find(Report.Id);

		if (reportFromDb != null)
        {
            //Delete image
			if (reportFromDb.ImagePath != null)
			{
				var pathToDeleteImage = _webHostEnvironment.WebRootPath + reportFromDb.ImagePath;
				if (System.IO.File.Exists(pathToDeleteImage))
				{
					System.IO.File.Delete(pathToDeleteImage);
				}
			}

			_db.UsersReports.Remove(reportFromDb);
            await _db.SaveChangesAsync();
            return RedirectToPage("ViewReports");
        }
        return Page();

    }
}
