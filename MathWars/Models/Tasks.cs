using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MathWars.Models;

public class Tasks
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "Pole tytuł jest wymagane")]
    public string Title { get; set; } = string.Empty;
    [Required(ErrorMessage = "Pole treść jest wymagane")]
    public string Content { get; set; } = string.Empty;
    public string? ImagePath { get; set; } = string.Empty;
    [Required(ErrorMessage = "Pole odpowiedź do zadania jest wymagane")]
    public string Answer { get; set; } = string.Empty;

    // Foreign Key to AnswerTypes
    [Required(ErrorMessage = "Musisz wybrać typ odpowiedzi")]
    public int AnswerTypeId { get; set; }
    [AllowNull]
    public AnswerTypes? AnswerType { get; set; }

    public DateTime Created { get; set; } = DateTime.Now;

    // Foreign Key to TasksCategory
    public List<TasksAndCategories> TasksAndCategories { get; set; } = new();
    public List<TasksCategory> Category { get; set; } =  new();

    
    [Range(1, 6, ErrorMessage = "Podaj wartość 1-6")]
    [Required(ErrorMessage = "Pole poziom trudności jest wymagane")]
    public int difficultyLevel { get; set; }
    [AllowNull]
    public ICollection<Answers> Answers { get; set; } = new List<Answers>();
}
