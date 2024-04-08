using MathWars.Entities;
using MathWars.Interfaces;
using MathWars.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace MathWars.Pages.Accounts;

[BindProperties]
public class ForgotPasswordModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IEmailSenderService _emailSenderService;

    public ForgotPasswordModel(UserManager<ApplicationUser> userManager, IConfiguration configuration, IEmailSenderService emailSenderService)
    {
        _userManager = userManager;
        _configuration = configuration;
        _emailSenderService = emailSenderService;
    }

    public bool IsEmailSent { get; set; } = false;
    [Required(ErrorMessage ="Musisz wype³niæ to pole"), EmailAddress, Display(Name ="Email na który siê zarejestrowa³eœ")]
    public string Email { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if ( ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(Email);

            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                if (!string.IsNullOrEmpty(token))
                {
                    await SendForgotPasswordEmailAsync(user, token);
                    IsEmailSent = true;
                    return Page();
                }
            }
            else 
            {
                ModelState.AddModelError("Email", "Nie ma u¿ytkownika o takim adresie e-mail");
                return Page(); 
            }
        }

        return Page();
    }

    private async Task SendForgotPasswordEmailAsync(ApplicationUser user, string token)
    {
        string appDomain = _configuration.GetSection("ApplicationDetails:AppDomain").Value;
        string confirmationLink = _configuration.GetSection("ApplicationDetails:ForgotPassword").Value;
        UserEmailOptions options = new UserEmailOptions()
        {
            ToEmails = new List<string>() { user.Email },
            PlaceHolders = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("{{UserName}}", user.UserName),
                new KeyValuePair<string, string>("{{Link}}", string.Format(appDomain + confirmationLink,user.Id,token))
            }
        };

        await _emailSenderService.SendEmailForForgotPassword(options);
    }
}
