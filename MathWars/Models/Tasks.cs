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

    // Foreign Key to TasksCategory
    public int CategoryId { get; set; }
    public TasksCategory Category { get; set; } = null!;

    [Required]
    public int difficultyLevel { get; set; }
    [AllowNull]
    public ICollection<Answers> Answers { get; set; } = new List<Answers>();
}
