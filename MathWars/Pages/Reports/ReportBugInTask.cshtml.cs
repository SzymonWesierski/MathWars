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

    [Required(ErrorMessage ="Przed wys�aniem muisz wype�ni� to pole"), Display(Name ="Wprowad� tre�� wiadomo�ci:")]
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
