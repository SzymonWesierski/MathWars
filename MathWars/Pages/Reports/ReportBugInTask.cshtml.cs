using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace MathWars.Pages.Reports;

[BindProperties]
[Authorize]
public class ReportBugInTaskModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IConfiguration _configuration;

    public ReportBugInTaskModel(ApplicationDbContext context,UserManager<ApplicationUser> userManager,
        IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
    {
        _context = context;
        _userManager = userManager;
        _webHostEnvironment = webHostEnvironment;
        _configuration = configuration;
    }

    public UsersReports UserReport { get; set; } = new UsersReports();
    public IFormFile? ImageFile { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        UserReport.TaskId = id.Value;

        var tempUser = await _userManager.GetUserAsync(User);

        if (tempUser == null)
        {
            return NotFound();
        }

        UserReport.UserId = tempUser.Id;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            // Handle image upload
            if (ImageFile != null && ImageFile.Length > 0)
            {
                // generating new uniqe file name 
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;

                var appDirectory = _webHostEnvironment.WebRootPath;

                var imageDirectory = _configuration.GetSection("ImagesDirectorys")
                    .GetValue<string>("reportPictures");

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
                    UserReport.ImagePath = filePath;
                }
            }

            await _context.UsersReports.AddAsync(UserReport);
            await _context.SaveChangesAsync();
            return RedirectToPage("/TaskPages/SolvingTask", new { id = UserReport.TaskId });
        }
        return Page();
    }
}
