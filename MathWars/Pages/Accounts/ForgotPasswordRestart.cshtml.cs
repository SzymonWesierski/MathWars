using MathWars.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace MathWars.Pages.Accounts;

[BindProperties]
public class ForgotPasswordRestartModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    public ForgotPasswordRestartModel(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [Required]
    public string UserId { get; set;}
    [Required]
    public string Token { get; set;}
    [Required, DataType(DataType.Password), Display(Name = "Nowe has³o")]
    public string NewPassword { get; set; }
    [Required, DataType(DataType.Password), Display(Name = "Powtórz nowe has³o")]
    [Compare("NewPassword")]
    public string ConfirmNewPassword { get; set; }
    public bool IsSuccess { get; set; } = false;

    public void OnGet(string uid, string token)
    {
        UserId = uid;
        Token = token;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!string.IsNullOrEmpty(UserId) && !string.IsNullOrEmpty(Token))
        {
            Token = Token.Replace(" ", "+");
            var result = await ResetPasswordAsync(UserId, Token, NewPassword);

            if (result.Succeeded)
            {
                IsSuccess = true;
                return Page();
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        return Page();
    }


        private async Task<IdentityResult> ResetPasswordAsync(string uid, string token, string newPassword)
    {
        return await _userManager.ResetPasswordAsync(await _userManager.FindByIdAsync(uid), token, newPassword);
    }
}
