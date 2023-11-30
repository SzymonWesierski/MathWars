using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MathWars.Pages.TaskPages;
[Authorize]
[BindProperties]
public class DeleteUserModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public ApplicationUser user;

    public DeleteUserModel(ApplicationDbContext db)
    {
        _db = db;
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
            _db.Users.Remove(userFromDb);
            await _db.SaveChangesAsync();
            return RedirectToPage("ViewTasksCategory");
        }
        return Page();

    }
}
