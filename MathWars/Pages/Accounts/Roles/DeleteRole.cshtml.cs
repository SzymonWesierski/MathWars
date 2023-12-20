using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MathWars.Pages.TaskPages;
[Authorize(Roles = "admin")]
[BindProperties]
public class DeleteRoleModel : PageModel
{
    private readonly RoleManager<IdentityRole> _roleManager;
    public IdentityRole? role { get; set; }

    public DeleteRoleModel(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }
    public async void OnGet(string id)
    {
        role = await _roleManager.FindByIdAsync(id);
    }

    public async Task<IActionResult> OnPost()
    {
        var roleToDelete = await _roleManager.FindByIdAsync(role.Id);

        if (roleToDelete != null)
        {
            var result = await _roleManager.DeleteAsync(roleToDelete);

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
