using Microsoft.AspNetCore.Identity;

namespace MathWars.Models;

public class ApplicationUser : IdentityUser
{
    public ICollection<Answers> Answers { get; set; } = new List<Answers>();
	public DateTime Created { get; set; } = DateTime.Now;
	public int Level { get; set; } = 1;
    public int Experience { get; set; } = 0;
    public int ExpToReachNewLvl { get; set; } = 20;
    public string? ProfileImagePath { get; set; } = string.Empty;
}


