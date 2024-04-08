using MathWars.Entities;
using MathWars.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace MathWars.Data
{
	public class Seed
	{
		public static async Task SeedUsers(UserManager<ApplicationUser> userManager,
			RoleManager<IdentityRole> roleManager)
		{
			if (await userManager.Users.AnyAsync()) return;

			var userData = await File.ReadAllTextAsync("Data/SeedData/UserSeedData.json");

			var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

			var users = JsonSerializer.Deserialize<List<ApplicationUser>>(userData);

			var roles = new List<IdentityRole>
			{
				new IdentityRole{ Name = "Admin"},
				new IdentityRole{ Name = "TaskManager"},
				new IdentityRole{ Name = "User"}
			};

			foreach (var role in roles)
			{
				await roleManager.CreateAsync(role);
			}

			var admin = new ApplicationUser
			{
				UserName = "admin",
				Email = "defaultAdmin@email.com",
				EmailConfirmed = true,
			};

			await userManager.CreateAsync(admin, "Admin1@");
			await userManager.AddToRoleAsync(admin, "Admin");

			foreach (var user in users)
			{
				user.UserName = user.UserName.ToLower();
				await userManager.CreateAsync(user, "MathUser123!");
				await userManager.AddToRoleAsync(user, "User");
			}
		}

		public static async Task SeedTaskCategory(ApplicationDbContext _db)
		{
            if (await _db.TasksCategory.AnyAsync()) return;

            var categoryData = await File.ReadAllTextAsync("Data/SeedData/TaskCategorySeedData.json");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var categories = JsonSerializer.Deserialize<List<TasksCategory>>(categoryData);

            foreach (var category in categories)
            {
				await _db.TasksCategory.AddAsync(category);
            }
           await _db.SaveChangesAsync();

        }

        public static async Task SeedTasks(ApplicationDbContext _db)
        {
            if (await _db.Tasks.AnyAsync()) return;

            var tasksData = await File.ReadAllTextAsync("Data/SeedData/TasksSeedData.json");
            var categoriesAndTaskData = await File.ReadAllTextAsync("Data/SeedData/TasksAndCategoriesSeedData.json");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var tasks = JsonSerializer.Deserialize<List<Tasks>>(tasksData);
            var categoriesAndTask = JsonSerializer.Deserialize<List<TasksAndCategories>>(categoriesAndTaskData);

            foreach (var task in tasks)
            {
                await _db.Tasks.AddAsync(task);
            }

            foreach (var ct in categoriesAndTask)
            {
                await _db.TasksAndCategories.AddAsync(ct);
            }

            await _db.SaveChangesAsync();
        }

        public static async Task SeedAnswersToTask(ApplicationDbContext _db)
        {
            if (await _db.AnswersToTasks.AnyAsync()) return;

            var answersToTaskData = await File.ReadAllTextAsync("Data/SeedData/AnswersToTaskSeedData.json");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var answersToTask = JsonSerializer.Deserialize<List<AnswersToTask>>(answersToTaskData);

            foreach (var answer in answersToTask)
            {
                await _db.AnswersToTasks.AddAsync(answer);
            }

            await _db.SaveChangesAsync();
        }
    }
}
