using System.ComponentModel.DataAnnotations;

namespace MathWars.Entities;

public class UsersReports
{
    [Key]
    public int Id { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public string Title { get; set; }
    public string Description { get; set; }
	public string ImageUrl { get; set; } = String.Empty;
	public string PublicImageId { get; set; } = String.Empty;
	public string UserId { get; set; }
    public int TaskId { get; set; } = 0;
}
