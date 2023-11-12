using MathWars.Models;
using MathWars.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MathWars.Pages.Accounts;

public class RegisterModel : PageModel
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;
    [BindProperty]
    public Register registerModel { get; set; }

    public RegisterModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) 
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
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
                await signInManager.SignInAsync(user, isPersistent: false);
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