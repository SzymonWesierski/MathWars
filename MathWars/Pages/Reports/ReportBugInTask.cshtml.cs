using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace MathWars.Pages.Reports;

[BindProperties]
[Authorize]
public class ReportBugInTaskModel : PageModel
{

    public ReportBugInTaskModel()
    {
        
    }

    [Required(ErrorMessage ="Przed wys³aniem muisz wype³niæ to pole"), Display(Name ="WprowadŸ treœæ wiadomoœci:")]
    public string EmailContent { get; set; }
    public int TaskId { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        TaskId = id.Value;

        return Page();
    }

    public ActionResult OnPost()
    {
        return Page();
    }
}
