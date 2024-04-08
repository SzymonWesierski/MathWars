using MathWars.Entities;
using MathWars.Interfaces;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MathWars.Pages.Accounts;

[BindProperties]
public class ConfirmEmailModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IEmailSenderService _emailSenderService;

    public ConfirmEmailModel(UserManager<ApplicationUser> userManager, IConfiguration configuration, IEmailSenderService emailSenderService)
    {
        _userManager = userManager;
        _configuration = configuration;
        _emailSenderService = emailSenderService;
    }

    public string Uid { get; set; }
    public bool IsResendMail { get; set; } = false;

    public async Task<IActionResult> OnGet(string uid)
    {
        var currentUser = await _userManager.GetUserAsync(User);

        if (currentUser != null)
        {
            return RedirectToPage("/Index");
        }

        Uid = uid;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync() 
    {
        var user = await _userManager.FindByIdAsync(Uid);

        if (user != null)
        {
            if(user.EmailConfirmed)
            {
                return RedirectToPage("/Index");
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            if (!string.IsNullOrEmpty(token))
            {
                await SendEmailConfirmationEmailAsync(user, token);
                IsResendMail = true;
                return Page();
            }
        }

        return Page();
    }
    private async Task SendEmailConfirmationEmailAsync(ApplicationUser user, string token)
    {
        string appDomain = _configuration.GetSection("ApplicationDetails:AppDomain").Value;
        string confirmationLink = _configuration.GetSection("ApplicationDetails:EmailConfirmation").Value;
        UserEmailOptions options = new UserEmailOptions()
        {
            ToEmails = new List<string>() { user.Email },
            PlaceHolders = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("{{UserName}}", user.UserName),
                new KeyValuePair<string, string>("{{Link}}", string.Format(appDomain + confirmationLink,user.Id,token))
            }
        };

        await _emailSenderService.SendEmailForEmailConfirmation(options);
    }

}
