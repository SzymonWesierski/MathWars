using MathWars.Data;
using MathWars.Models;
using MathWars.Services;
using MathWars.StartupInitializers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
		builder.Configuration.GetConnectionString("DefaultConnection")
	));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    var section = builder.Configuration.GetSection("ApplicationPasswordRequirements");

    options.Password.RequiredLength = section.GetValue<int>("RequiredLength");
    options.Password.RequiredUniqueChars = section.GetValue<int>("RequiredUniqueChars");
    options.Password.RequireDigit = section.GetValue<bool>("RequireDigit");
    options.Password.RequireLowercase = section.GetValue<bool>("RequireLowercase");
    options.Password.RequireNonAlphanumeric = section.GetValue<bool>("RequireNonAlphanumeric");
    options.Password.RequireUppercase = section.GetValue<bool>("RequireUppercase");
    options.SignIn.RequireConfirmedEmail = section.GetValue<bool>("RequireConfirmedEmail");
});

builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();

builder.Services.Configure<SMTPConfigModel>(builder.Configuration.GetSection("SMTPConfig"));

builder.Services.ConfigureApplicationCookie(config =>
{
	config.LoginPath = "/IndexGuest";
});

builder.Services.AddSession();

// Initializing basic roles
var roleManager = builder.Services.BuildServiceProvider().GetService<RoleManager<IdentityRole>>();
var configuration = builder.Configuration.GetSection("ApplicationRoles").GetChildren();
foreach (var role in configuration)
{
    RoleInitializer.InitializeRoleAsync(roleManager, role.Value).Wait();
}

// Check if default admin user exist
var userManager = builder.Services.BuildServiceProvider().GetService<UserManager<ApplicationUser>>();
await AdminInitializer.InitializeUserAsync(userManager, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapRazorPages();

app.Run();
