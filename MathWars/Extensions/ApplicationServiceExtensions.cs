using MathWars.Data;
using MathWars.Helpers;
using MathWars.Interfaces;
using MathWars.Models;
using MathWars.Repository;
using MathWars.Services;
using Microsoft.EntityFrameworkCore;

namespace MathWars.Extensions
{
    public static class ApplicationServiceExtensions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
		{
			services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
				config.GetConnectionString("DefaultConnection"),
				providerOptions => providerOptions.EnableRetryOnFailure()));

			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

			services.Configure<SMTPConfigModel>(config.GetSection("SMTPConfig"));
            services.AddScoped<IEmailSenderService, EmailSenderService>();
			
            services.Configure<CloudinarySettingsModel>(config.GetSection("CloudinarySettings"));
            services.AddScoped<IPhotoService, PhotoService>();

            services.AddScoped<IImageOnServerService, ImageOnServerService>();

			services.AddScoped<IConfigurationService, ConfigurationService>();

			services.AddScoped<ISessionService, SessionService>();

            services.AddScoped<ITaskCreatorService, TaskCreatorService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

			services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromMinutes(20);
			});

            services.AddHttpClient<IChatGptService, ChatGptService>();
            services.AddLogging(config =>
            {
                config.AddConsole();
                config.AddDebug();
            });

            return services;
		}
	}
}
