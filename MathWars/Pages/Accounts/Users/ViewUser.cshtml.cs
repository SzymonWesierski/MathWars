using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MathWars.Models;
using MathWars.Helpers;
using MathWars.Entities;

namespace MathWars.Pages.TaskPages;
[Authorize(Policy = "RequireAdminRole")]
public class ViewUserModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
	private readonly IConfiguration _configuration;

	public ViewUserModel(ILogger<IndexModel> logger, UserManager<ApplicationUser> userManager, 
        IConfiguration configuration)
    {
        _logger = logger;
        _userManager = userManager;
		_configuration = configuration;
    }

	public IEnumerable<ApplicationUser> Users { get; set; }
	public PaginatedList<UserWithRoleModel> UserWithRoleList { get; set; }

    public async Task OnGetAsync(int? pageIndex)
    {
        int pageSize = _configuration.GetSection("NumberOfElementsInList").GetValue<int>("Users");
        Users = await _userManager.Users.OrderByDescending(u => u.Created).ToListAsync();

        List<UserWithRoleModel> userWithRoleList = await PrepareUserWithRoleList();

        int count = userWithRoleList.Count();
        var items = userWithRoleList.Skip(((pageIndex ?? 1) - 1) * pageSize).Take(pageSize).ToList();

        UserWithRoleList = new PaginatedList<UserWithRoleModel>(items, count, pageIndex ?? 1, pageSize);
    }

    private async Task<List<UserWithRoleModel>> PrepareUserWithRoleList()
    {
        List<UserWithRoleModel> userWithRoleIQ = new List<UserWithRoleModel>();
        foreach (var user in Users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();
            var userAndRole = new UserWithRoleModel
            {
                user = user,
                RoleName = role
            };
            userWithRoleIQ.Add(userAndRole);
        }
        return userWithRoleIQ;
    }

}