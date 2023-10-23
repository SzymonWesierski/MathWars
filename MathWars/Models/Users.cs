namespace MathWars.Models;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime RegistrationDate { get; set; }
    public int ExperiencePoints { get; set; }
    public int Rank { get; set; }
    public List<Answer> Answers { get; set; }
}
