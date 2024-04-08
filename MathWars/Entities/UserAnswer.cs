using System.ComponentModel.DataAnnotations;

namespace MathWars.Entities;

public class UserAnswer
{
    [Key]
    public int Id { get; set; }
    public DateTime SubmissionDate { get; set; } = DateTime.Now;
    public string WhiteBoardPhotoUrl { get; set; }
    public string PublicWhiteBoardPhotoId { get; set; }
    public bool IsSolvedCorrect { get; set; }

    // Foreign Key to Users
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    // Foreign Key to Tasks
    public int TaskId { get; set; }
    public Tasks Task { get; set; }
}
