using Microsoft.AspNetCore.Mvc.RazorPages;
using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MathWars.ViewModels;
using System.Linq;

namespace MathWars.Pages.TaskPages;
[Authorize(Roles = "admin")]
public class ViewUserModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> userManager;
	private readonly IConfiguration _configuration;

	public ViewUserModel(ApplicationDbContext db, ILogger<IndexModel> logger, UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _db = db;
        _logger = logger;
        this.userManager = userManager;
		_configuration = configuration;
    }

	public IEnumerable<ApplicationUser> Users { get; set; }
	public PaginatedList<UserWithRole> UserWithRoleList { get; set; }

    public async Task OnGetAsync(int? pageIndex)
    {
        int pageSize = _configuration.GetSection("NumberOfElementsInList").GetValue<int>("Users");
        Users = await userManager.Users.ToListAsync();

        List<UserWithRole> userWithRoleList = await PrepareUserWithRoleList();

        int count = userWithRoleList.Count();
        var items = userWithRoleList.Skip(((pageIndex ?? 1) - 1) * pageSize).Take(pageSize).ToList();

        UserWithRoleList = new PaginatedList<UserWithRole>(items, count, pageIndex ?? 1, pageSize);
    }

    private async Task<List<UserWithRole>> PrepareUserWithRoleList()
    {
        List<UserWithRole> userWithRoleIQ = new List<UserWithRole>();
        foreach (var user in Users)
        {
            var roles = await userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();
            var userAndRole = new UserWithRole
            {
                user = user,
                RoleName = role
            };
            userWithRoleIQ.Add(userAndRole);
        }
        return userWithRoleIQ;
    }

}