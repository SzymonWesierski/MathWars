using MathWars.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MathWars.Pages.Accounts;
public class ConfirmedEmailModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    public ConfirmedEmailModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public bool Success { get; set; } = false;

    public async Task<IActionResult> OnGet(string uid, string token)
    {
        if(!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(token))
        {
            token = token.Replace(" ","+");
            var result = await ConfirmEmailAsync(uid, token);

            if(result.Succeeded)
            {
                var user = await _userManager.FindByIdAsync(uid);
                if (user != null)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    Success = true;
                    return Page();
                }
            }
        }
        Success = false;

        return Page();
    }

    private async Task<IdentityResult> ConfirmEmailAsync(string uid, string token)
    {
        return await _userManager.ConfirmEmailAsync(await _userManager.FindByIdAsync(uid), token);
    }
}
