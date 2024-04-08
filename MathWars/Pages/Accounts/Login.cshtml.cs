using MathWars.Entities;
using MathWars.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MathWars.Pages.Accounts
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginModel(SignInManager<ApplicationUser> signInManager) 
        { 
            _signInManager = signInManager;
        }

		[BindProperty]
		public LoginUserModel loginModel { get; set; }

		public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var identityResult = await _signInManager
                    .PasswordSignInAsync(loginModel.UserName, loginModel.Password, loginModel.RememberMe, false);

                if (identityResult.Succeeded)
                {
                    if(returnUrl == null || returnUrl == "/")
                    {
                        return RedirectToPage("/Index");
                    }
                    else
                    {
                        return RedirectToPage(returnUrl);
                    }
                }
                if (identityResult.IsNotAllowed)
                {
                    ModelState.AddModelError("loginModel.Password", "Nie zatwierdzi³eœ jeszcze maila, którego Ci wys³aliœmy :)");
                }
                else
                {
                    ModelState.AddModelError("loginModel.Password", "Nazwa u¿ytkownika lub has³o jest nieprawid³owe");
                }
            }
            return Page();
            
        }
    }
}
