using MathWars.Entities;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace MathWars.Pages.Accounts.Users;

[BindProperties]
[Authorize(Policy = "RequireAdminRole")]
public class CreateUserModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
	public RegisterUserModel createAccount { get; set; }
    public IEnumerable<IdentityRole> Roles { get; set; }
    public IdentityRole Role { get; set; }


    public CreateUserModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) 
    {
        _userManager = userManager;
        _roleManager = roleManager;
	}
    public void OnGet()
    {
        Roles = _roleManager.Roles;
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
            var result = await _userManager.CreateAsync(user, createAccount.Password);
            if (result.Succeeded) 
            {
                Role = await _roleManager.FindByIdAsync(Role.Id);
                await _userManager.AddToRoleAsync(user, Role.Name);
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