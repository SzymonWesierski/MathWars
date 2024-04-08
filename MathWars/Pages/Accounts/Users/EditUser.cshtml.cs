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
public class EditUserModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
	private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _uow;

	public EditUserModel(UserManager<ApplicationUser> userManager, IUnitOfWork uow, 
        IConfiguration configuration, RoleManager<IdentityRole> roleManager)
    {
        _uow = uow;
        _configuration = configuration;
        _userManager = userManager;
        _roleManager = roleManager;
    }

	public ApplicationUser user { get; set; }
	public IdentityRole NewRole { get; set; }
	public string CurrentRole { get; set; }
	public IEnumerable<IdentityRole> Roles { get; set; }

	public async Task OnGetAsync(string id)
    {
        user = await _uow.UserRepository.GetUserByIdAsync(id);
        Roles = _roleManager.Roles;
		var roles = await _userManager.GetRolesAsync(user);
		CurrentRole = roles.FirstOrDefault();
	}

    public async Task<IActionResult> OnPost()
    {
        user = await UpdatingUser();

        if (UserValidation())
        {
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();
            var formRole = await _roleManager.FindByIdAsync(NewRole.Id);

            if (role != null)
            {
                if(role != formRole.Name)
                    foreach (var roleName in roles)
                    {
                        await _userManager.RemoveFromRoleAsync(user, roleName);
                    }
                await _userManager.AddToRoleAsync(user, formRole.Name);
            }
            
            if(role == null && formRole != null)
            {
                await _userManager.AddToRoleAsync(user, formRole.Name);
            }

            await _userManager.UpdateAsync(user);

            if(await _uow.Complete()) return RedirectToPage("ViewUser");

            return Page();
		}
        return Page();
    }

    private bool UserValidation()
    {
        bool result = true;
        // User validation here
        return result;
    }

    private async Task<ApplicationUser> UpdatingUser()
    {
		var userDB = await _uow.UserRepository.GetUserByIdAsync(user.Id);
		if (userDB != null)
        {
            userDB.Level = user.Level;
            userDB.ExpToReachNewLvl = userDB.Level * GetLevelMultiplier();
            userDB.Experience = user.Experience;
            userDB.UserName = user.UserName;
            userDB.Email = user.Email;
            return userDB;
        }
        return userDB;
    }
    private int GetLevelMultiplier()
    {
        if (_configuration != null)
        {
            var lvlMultiplierSection = _configuration.GetSection("Experience&Level");
            if (lvlMultiplierSection.Exists())
            {
                return lvlMultiplierSection.GetValue<int>("lvlMultiplier");
            }
        }

        ModelState.AddModelError(string.Empty, "Couldn't find the configuration for 'lvlMultiplier' !!!");
        return 0;
    }
}
