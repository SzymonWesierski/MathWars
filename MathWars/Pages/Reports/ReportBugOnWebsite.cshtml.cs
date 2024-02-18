using MathWars.Models;
using MathWars.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace MathWars.Pages.Reports;

[BindProperties]
[Authorize]
public class ReportBugOnWebsiteModel : PageModel
{
    private readonly IEmailSenderService _emailSenderService;
    public ReportBugOnWebsiteModel(IEmailSenderService emailSenderService)
    {
        _emailSenderService = emailSenderService;
    }


    [Required(ErrorMessage = "Przed wys³aniem muisz wype³niæ to pole"), Display(Name = "WprowadŸ treœæ wiadomoœci:")]
    public string EmailContent { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            UserEmailOptions options = new UserEmailOptions
            {
                ToEmails = new List<string> { "sz425@wp.pl" },
                PlaceHolders = new List<KeyValuePair<string, string>>() 
                {
                    new KeyValuePair<string, string>("{{UserName}}", "Szymon")
                }
            };

            await _emailSenderService.SendTestEmail(options);

            return Page();
        }
        return Page();
    }
}
