using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace MathWars.Pages.TaskPages;
[Authorize(Roles = "admin")]
public class ViewRoleModel : PageModel
{
    private readonly RoleManager<IdentityRole> _roleManager;
    public IEnumerable<IdentityRole?> roles { get; set; }

    public ViewRoleModel(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }
    public void OnGet()
    {
        roles = _roleManager.Roles;
    }
}