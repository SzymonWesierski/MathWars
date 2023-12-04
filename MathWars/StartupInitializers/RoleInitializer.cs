using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MathWars.StartupInitializers;
public static class RoleInitializer
{
    public static async Task InitializeRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
    {
        bool roleExists = await roleManager.RoleExistsAsync(roleName);

        if (!roleExists)
        {
            var role = new IdentityRole { Name = roleName };
            var result = await roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                throw new Exception($"Error creating role '{roleName}'.");
            }
        }
    }
}
