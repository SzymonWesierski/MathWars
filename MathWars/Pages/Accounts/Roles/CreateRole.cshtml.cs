using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MathWars.Pages.TaskPages;
[Authorize(Policy = "RequireAdminRole")]
[BindProperties]
public class CreateRoleModel : PageModel
{
    private readonly RoleManager<IdentityRole> _roleManager;
    public IdentityRole role { get; set; }
    public CreateRoleModel(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            var result =  await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToPage("ViewRole");
            }
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        return Page();
    }
}
