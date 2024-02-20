using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace MathWars.Pages.Accounts.Profile;

[Authorize]
[BindProperties]
public class ChangePasswordModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public ChangePasswordModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;

    }

    [Required(ErrorMessage = "Aktualne has³o jest wymagane"), DataType(DataType.Password), Display(Name = "Aktualne has³o:")]
    public string CurrentPassword {  get; set; }

    [Required(ErrorMessage = "Nowe has³o jest wymagane"), DataType(DataType.Password), Display(Name = "Nowe has³o:")]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "Powtórz nowe has³o jest wymagane"), DataType(DataType.Password), Display(Name = "Powtórz nowe has³o:")]
    [Compare("NewPassword", ErrorMessage = "Nowe i powtórzone has³o musi byæ takie samo")]
    public string ConfirmPassword { get; set; }
    public string Uid {  get; set; }

    public void OnGet(string uid)
    {
        Uid = uid;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByIdAsync(Uid) ?? new ApplicationUser();

            if (user == null) return NotFound();

            var result = await _userManager.ChangePasswordAsync(user, CurrentPassword, NewPassword);

            if (result.Succeeded)
            {
                await _signInManager.SignOutAsync();
                return RedirectToPage("/Accounts/Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();

        }
        return Page();
    }
}
