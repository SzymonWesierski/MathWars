using MathWars.Entities;
using MathWars.Interfaces;
using MathWars.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MathWars.Pages.Accounts;
[BindProperties]
public class RegisterModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
	private readonly IConfiguration _configuration;
    private readonly IEmailSenderService _emailSenderService;
	private readonly ILogger _logger;
	public RegisterUserModel registerModel { get; set; }

    public RegisterModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, 
        IConfiguration configuration, IEmailSenderService emailSenderService, ILogger<RegisterModel> logger) 
    {
		_logger = logger;
		_userManager = userManager;
        _signInManager = signInManager;
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
                ProfileImageUrl = _configuration.GetSection("ProfilePicture")
                    .GetValue<string>("defaultProfilePicture")
            };

            var result = await _userManager.CreateAsync(user,registerModel.Password);

            if (result.Succeeded) 
            {
                string roleName = GetUserRoleName();

                if(string.IsNullOrEmpty(roleName)) return BadRequest();

                var isAddToRole = await _userManager.AddToRoleAsync(user, roleName);

                if (roleName != string.Empty && isAddToRole.Succeeded) 
                {
					var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    if (!string.IsNullOrEmpty(token))
                    {
                        await SendEmailConfirmationEmailAsync(user, token);
                        return RedirectToPage("/Accounts/ConfirmEmail", new { uid = user.Id });
                    }
				}

				_logger.LogError("Can't add to role! RegisterModel.cshtml.cs -> OnPostAsync()");
				ModelState.AddModelError(string.Empty, "Can't add to role!");
			}

			ValidationRegistrationErrors(result);
		}
        return Page();
    }

	private string GetUserRoleName()
	{
		if (_configuration != null)
		{
			var userRoleName = _configuration.GetSection("ApplicationRoles");
			if (userRoleName.Exists())
			{
				return userRoleName.GetValue<string>("User");
			}
		}

        _logger.LogError("Can't get user role configuration! RegisterModel.cshtml.cs -> GetUserRoleName()");

		ModelState.AddModelError(string.Empty, "Can't get user role configuration!");

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

	private IdentityResult ValidationRegistrationErrors(IdentityResult result)
	{
		foreach (var error in result.Errors)
		{
			string customErrorMessage;

			switch (error.Code)
			{
				case "DuplicateUserName":
					customErrorMessage = "Nazwa u¿ytkownika jest ju¿ u¿ywana.";
					ModelState.AddModelError("registerModel.UserName", customErrorMessage);
					break;

				case "PasswordRequiresDigit":
					customErrorMessage = "Has³o musi zawieraæ co najmniej jedn¹ cyfrê.";
					ModelState.AddModelError("registerModel.Password", customErrorMessage);
					break;

				case "PasswordRequiresLower":
					customErrorMessage = "Has³o musi zawieraæ co najmniej jedn¹ ma³¹ literê.";
					ModelState.AddModelError("registerModel.Password", customErrorMessage);
					break;

				case "PasswordRequiresUpper":
					customErrorMessage = "Has³o musi zawieraæ co najmniej jedn¹ du¿¹ literê.";
					ModelState.AddModelError("registerModel.Password", customErrorMessage);
					break;

				case "PasswordRequiresNonAlphanumeric":
					customErrorMessage = "Has³o musi zawieraæ co najmniej jeden znak niealfanumeryczny.";
					ModelState.AddModelError("registerModel.Password", customErrorMessage);
					break;

				case "PasswordTooShort":
					customErrorMessage = "Has³o jest zbyt krótkie.";
					ModelState.AddModelError("registerModel.Password", customErrorMessage);
					break;

				default:
					ModelState.AddModelError("registerModel.ConfirmPassword", error.Description);
					break;
			}
		}

		return result;
	}
}