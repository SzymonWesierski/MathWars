using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MathWars.Models;

public class Tasks
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Content { get; set; }
    [Required]
    public double Answer { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    [Required]
    public string category { get; set; }
    [Required]
    public int difficultyLevel { get; set; }
    [AllowNull]
    public ICollection<Answers> Answers { get; set; } = new List<Answers>();
}
