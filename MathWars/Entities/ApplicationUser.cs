using Microsoft.AspNetCore.Identity;

namespace MathWars.Entities;

public class ApplicationUser : IdentityUser
{
    public DateTime Created { get; set; } = DateTime.Now;
    public int Level { get; set; } = 1;
    public int Experience { get; set; } = 0;
    public int ExpToReachNewLvl { get; set; } = 20;
    public string ProfileImageUrl { get; set; } = string.Empty;
    public string PublicProfileImageId { get; set; }
	public List<TaskRating> TaskRatings { get; set; }
	public List<UserAnswer> Answers { get; set; }
}


