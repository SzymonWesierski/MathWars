using System.ComponentModel.DataAnnotations;

namespace MathWars.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    public DateTime RegistrationDate { get; set; }
    public int ExperiencePoints { get; set; }
    public int Rank { get; set; }
    public List<Answer> Answers { get; set; }
}
