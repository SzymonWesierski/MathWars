using MathWars.Models;
using MathWars.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace MathWars.Pages.Accounts.Users;

[BindProperties]
[Authorize(Roles = "admin")]
public class CreateUserModel : PageModel
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly RoleManager<IdentityRole> roleManager;
	public Register createAccount { get; set; }
    public IEnumerable<IdentityRole> Roles { get; set; }
    public IdentityRole Role { get; set; }


    public CreateUserModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager) 
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.roleManager = roleManager;
	}
    public void OnGet()
    {
        Roles = roleManager.Roles;
    }

    public async Task<IActionResult> OnPostAsync()
    { 
        if (ModelState.IsValid)
        {

            var user = new ApplicationUser()
            {
                UserName = createAccount.UserName,
                Email = createAccount.Email,
            };
            var result = await userManager.CreateAsync(user, createAccount.Password);
            if (result.Succeeded) 
            {
                Role = await roleManager.FindByIdAsync(Role.Id);
                await userManager.AddToRoleAsync(user, Role.Name);
                return RedirectToPage("/Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return Page();
    }

	
}