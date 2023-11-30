using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace MathWars.Pages.TaskPages;
[Authorize]
[BindProperties]
public class EditUserModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public ApplicationUser user { get; set; }
    private readonly IConfiguration _configuration;

    public EditUserModel(ApplicationDbContext db, IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
    }
    public void OnGet(string id)
    {
        user = _db.Users.Find(id);
    }

    public async Task<IActionResult> OnPost()
    {
        user = UpdatingUser();

        if (UserValidation())
        {
            _db.Users.Update(user);
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
