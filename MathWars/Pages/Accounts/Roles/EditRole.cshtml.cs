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
public class EditRoleModel : PageModel
{
    private readonly RoleManager<IdentityRole> _roleManager;
    public IdentityRole role { get; set; }

    public EditRoleModel(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }
    public async void OnGet(string id)
    {
        role = await _roleManager.FindByIdAsync(id);
    }

    public async Task<IActionResult> OnPost()
    {
        var roleToUpdate = await _roleManager.FindByIdAsync(role.Id);

        if (roleToUpdate != null)
        {
            roleToUpdate.Name = role.Name;

            var result = await _roleManager.UpdateAsync(roleToUpdate);

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