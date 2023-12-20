using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Threading.Tasks;

namespace MathWars.Pages.TaskPages;
[Authorize(Roles = "admin")]
[BindProperties]
public class DeleteUserModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> userManager;
    public ApplicationUser user { get; set; }

    public DeleteUserModel(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
    {
        _db = db;
        this.userManager = userManager;
    }
    public void OnGet(string id)
    {
        user = _db.Users.Find(id);
    }

    public async Task<IActionResult> OnPost()
    {
        var userFromDb = _db.Users.Find(user.Id);
        if (userFromDb != null)
        {
            var roles = await userManager.GetRolesAsync(userFromDb);
            foreach (var roleName in roles)
            {
                await userManager.RemoveFromRoleAsync(userFromDb, roleName);
            }
            _db.Users.Remove(userFromDb);
            await _db.SaveChangesAsync();
            return RedirectToPage("ViewUser");
        }
        return Page();

    }
}
