using MathWars.Data;
using MathWars.Entities;
using Microsoft.AspNetCore.Identity;

namespace MathWars.Extensions
{
    public static class IdentityServiceExtensions
	{
		public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config) 
		{
			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

			services.Configure<IdentityOptions>(options =>
			{
				var section = config.GetSection("ApplicationPasswordRequirements");

				options.Password.RequiredLength = section.GetValue<int>("RequiredLength");
				options.Password.RequiredUniqueChars = section.GetValue<int>("RequiredUniqueChars");
				options.Password.RequireDigit = section.GetValue<bool>("RequireDigit");
				options.Password.RequireLowercase = section.GetValue<bool>("RequireLowercase");
				options.Password.RequireNonAlphanumeric = section.GetValue<bool>("RequireNonAlphanumeric");
				options.Password.RequireUppercase = section.GetValue<bool>("RequireUppercase");
				options.SignIn.RequireConfirmedEmail = section.GetValue<bool>("RequireConfirmedEmail");
			});

			services.ConfigureApplicationCookie(config =>
			{
				config.LoginPath = "/IndexGuest";
				config.AccessDeniedPath = "/Accounts/Login";
			});

			services.AddSession();

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
                opt.AddPolicy("RequireAdminOrManagerRole", policy => policy.RequireRole("Admin", "TaskManager"));
            });

            return services;
		}
	}
}
