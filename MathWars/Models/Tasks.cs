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

    [AllowNull]
    public string? ImagePath { get; set; } = string.Empty;
    [Required]
    public string Answer { get; set; }

	// Foreign Key to AnswerTypes
	[Required]
    public int AnswerTypeId { get; set; }
    public AnswerTypes AnswerType { get; set; }

    public DateTime Created { get; set; } = DateTime.Now;

    // Foreign Key to TasksCategory
    public List<TasksAndCategories> TasksAndCategories { get; set; } = new();
    public List<TasksCategory> Category { get; set; } =  new();

    [Required]
    [Range(1, 6, ErrorMessage = "Please enter a valid number (1-6).")]
    public int difficultyLevel { get; set; }
    [AllowNull]
    public ICollection<Answers> Answers { get; set; } = new List<Answers>();
}
