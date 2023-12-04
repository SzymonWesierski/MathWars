using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace MathWars.Pages.TaskPages;
[Authorize(Roles = "admin")]
[BindProperties]
public class EditUserModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly RoleManager<IdentityRole> roleManager;
    public ApplicationUser user { get; set; }
    private readonly IConfiguration _configuration;
    public IdentityRole NewRole { get; set; }
	public string CurrentRole { get; set; }
	public IEnumerable<IdentityRole> Roles { get; set; }

    public EditUserModel(UserManager<ApplicationUser> userManager, ApplicationDbContext db, IConfiguration configuration, RoleManager<IdentityRole> roleManager)
    {
        _db = db;
        _configuration = configuration;
        this.userManager = userManager;
        this.roleManager = roleManager;
    }
    public async Task OnGetAsync(string id)
    {
        user = _db.Users.Find(id);
        Roles = roleManager.Roles;
		var roles = await userManager.GetRolesAsync(user);
		CurrentRole = roles.FirstOrDefault();
	}

    public async Task<IActionResult> OnPost()
    {
        user = UpdatingUser();

        if (UserValidation())
        {
            var roles = await userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();
            var formRole = await roleManager.FindByIdAsync(NewRole.Id);

            if (role != null)
            {
                if(role != formRole.Name)
                    foreach (var roleName in roles)
                    {
                        await userManager.RemoveFromRoleAsync(user, roleName);
                    }
                await userManager.AddToRoleAsync(user, formRole.Name);
            }
            
            if(role == null && formRole != null)
            {
                await userManager.AddToRoleAsync(user, formRole.Name);
            }

            await userManager.UpdateAsync(user);
            await _db.SaveChangesAsync();
            return RedirectToPage("ViewUser");
        }
        return Page();
    }

    private bool UserValidation()
    {
        bool result = true;
        // User validation here
        return result;
    }

    private ApplicationUser UpdatingUser()
    {
        var userDB = _db.Users.Find(user.Id);
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
