using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MathWars.Migrations;
using MathWars.ViewModels;
using System.Threading.Tasks;

namespace MathWars.Pages.TaskPages;
[Authorize(Roles = "admin")]
public class ViewUserModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> userManager;
    private IEnumerable<ApplicationUser> Users { get; set; }
    public List<UserWithRole> UserWithRoleList { get; set; } = new List<UserWithRole>();

	public ViewUserModel(ApplicationDbContext db, ILogger<IndexModel> logger, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _logger = logger;
        this.userManager = userManager;
    }

	public async Task OnGet()
	{

		Users = await userManager.Users.ToListAsync();
		await PrepareUserWithRoleList();

		
	}

	private async Task PrepareUserWithRoleList()
	{
		foreach (var user in Users)
		{
			var roles = await userManager.GetRolesAsync(user);
			var role = roles.FirstOrDefault();
			var userAndRole = new UserWithRole
			{
				user = user,
				RoleName = role
			};
			UserWithRoleList.Add(userAndRole);
		}
	}
}