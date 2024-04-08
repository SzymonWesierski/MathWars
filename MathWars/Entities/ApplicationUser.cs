using Microsoft.AspNetCore.Identity;

namespace MathWars.Entities;

public class ApplicationUser : IdentityUser
{
    public ICollection<UserAnswer> Answers { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public int Level { get; set; } = 1;
    public int Experience { get; set; } = 0;
    public int ExpToReachNewLvl { get; set; } = 20;
    public string ProfileImageUrl { get; set; } = string.Empty;
    public string PublicProfileImageId { get; set; }
}


