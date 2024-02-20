using MathWars.Models;
using MathWars.Services;
using MathWars.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MathWars.Pages.Accounts;
[BindProperties]
public class RegisterModel : PageModel
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;
	private readonly IConfiguration _configuration;
    private readonly IEmailSenderService _emailSenderService;
    public Register registerModel { get; set; }

    public RegisterModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, IEmailSenderService emailSenderService) 
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
		_configuration = configuration;
        _emailSenderService = emailSenderService;

    }
    public void OnGet()
    {

    }

    public async Task<IActionResult> OnPostAsync()
    { 
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser()
            {
                UserName = registerModel.UserName,
                Email = registerModel.Email,   
                ProfileImagePath = _configuration.GetSection("ProfilePicture").GetValue<string>("defaultProfilePicture")
            };
            var result = await userManager.CreateAsync(user,registerModel.Password);
            if (result.Succeeded) 
            {
                string roleName = GetUserRoleName();
                var isAddToRole = await userManager.AddToRoleAsync(user, roleName);
                if (roleName != string.Empty && isAddToRole.Succeeded) 
                {
					var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    if (!string.IsNullOrEmpty(token))
                    {
                        await SendEmailConfirmationEmailAsync(user, token);
                        return RedirectToPage("/Accounts/ConfirmEmail", new { uid = user.Id });
                    }
				}
				ModelState.AddModelError(string.Empty, "Can't get role name");
			}

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return Page();
    }

	private string GetUserRoleName()
	{
		if (_configuration != null)
		{
			var expMultiplierSection = _configuration.GetSection("ApplicationRoles");
			if (expMultiplierSection.Exists())
			{
				return expMultiplierSection.GetValue<string>("User");
			}
		}

		ModelState.AddModelError(string.Empty, "Couldn't find the configuration for 'User' !!!");
		return string.Empty;
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