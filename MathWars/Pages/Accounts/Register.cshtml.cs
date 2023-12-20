using MathWars.Models;
using MathWars.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MathWars.Pages.Accounts;

public class RegisterModel : PageModel
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;
	private readonly IConfiguration _configuration;
	[BindProperty]
    public Register registerModel { get; set; }

    public RegisterModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration) 
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
		_configuration = configuration;
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
            };
            var result = await userManager.CreateAsync(user,registerModel.Password);
            if (result.Succeeded) 
            {
                string roleName = GetUserRoleName();
                if (roleName != string.Empty) 
                {
					await userManager.AddToRoleAsync(user, roleName);
					await signInManager.SignInAsync(user, isPersistent: false);
					return RedirectToPage("/Index");
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
}