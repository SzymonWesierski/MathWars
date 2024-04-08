using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MathWars.Data;
using MathWars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MathWars.Helpers;

namespace MathWars.Pages.TaskPages;
[Authorize(Policy = "RequireAdminOrManagerRole")]
public class ViewRoleModel : PageModel
{
    private readonly RoleManager<IdentityRole> _roleManager;
	private readonly IConfiguration _configuration;

	public ViewRoleModel(RoleManager<IdentityRole> roleManager, IConfiguration configuration)
	{
		_roleManager = roleManager;
		_configuration = configuration;
	}

	public PaginatedList<IdentityRole> Roles { get; set; }

	public async Task OnGetAsync(int? pageIndex)
	{
		int pageSize = _configuration.GetSection("NumberOfElementsInList").GetValue<int>("Roles");
		IQueryable<IdentityRole> roleQuery = _roleManager.Roles;

		Roles = await PaginatedList<IdentityRole>.CreateAsync(roleQuery, pageIndex ?? 1, pageSize);
	}
}