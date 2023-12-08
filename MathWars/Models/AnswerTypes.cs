using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MathWars.Models;

public class AnswerTypes
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string FormatExplanation { get; set; }
    [Required]
    public int HowManyCorrectAnswers { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public ICollection<Tasks> Tasks { get; set; } = new List<Tasks>();
}
