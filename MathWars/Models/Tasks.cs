using System.ComponentModel.DataAnnotations;

namespace MathWars.Models;

public class Tasks
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Content { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public string category { get; set; }
    [Required]
    public int difficultyLevel { get; set; }

    public ICollection<Answers> Answers { get; set; }
}
