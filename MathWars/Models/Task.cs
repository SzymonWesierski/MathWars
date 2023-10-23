namespace MathWars.Models;

public class Task
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public string category { get; set; }
    public int difficultyLevel { get; set; }
}
