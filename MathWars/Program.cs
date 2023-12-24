using MathWars.Data;
using MathWars.Models;
using MathWars.StartupInitializers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static System.Formats.Asn1.AsnWriter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
		builder.Configuration.GetConnectionString("DefaultConnection")
	));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

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
