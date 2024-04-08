using MathWars.Data;
using MathWars.Entities;
using MathWars.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MathWars.Pages.TaskPages;
[Authorize(Policy = "RequireAdminRole")]
[BindProperties]
public class DeleteUserModel : PageModel
{
    private readonly IUnitOfWork _uow;
    private readonly UserManager<ApplicationUser> _userManager;

    public DeleteUserModel(IUnitOfWork uow, UserManager<ApplicationUser> userManager)
    {
        _uow = uow;
        _userManager = userManager;
    }

    public ApplicationUser User { get; set; }

    public async Task<IActionResult> OnGetAsync(string id)
    {
        User = await _uow.UserRepository.GetUserByIdAsync(id);
        return Page();
    }


    public async Task<IActionResult> OnPost()
    {
        var userFromDb = await _uow.UserRepository.GetUserByIdAsync(User.Id);
        if (userFromDb != null)
        {
            var roles = await _userManager.GetRolesAsync(userFromDb);
            foreach (var roleName in roles)
            {
                await _userManager.RemoveFromRoleAsync(userFromDb, roleName);
            }
            

            return RedirectToPage("ViewUser");
        }
        return Page();

    }
}
