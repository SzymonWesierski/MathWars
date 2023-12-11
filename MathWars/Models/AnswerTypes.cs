using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MathWars.Models;

public class AnswerTypes
{
    [Key]
    public int Id { get; set; }
    //[Required(ErrorMessage ="Pole nazwy jest wymagane")]
    public string? Name { get; set; }
    //[Required(ErrorMessage = "Pole wyjaśnienia typu odpowiedzi jest wymagane")]
    public string? FormatExplanation { get; set; }
    [Required(ErrorMessage = "Pole ilość poprawnych odpowiedzi jest wymagane")]
    public int HowManyCorrectAnswers { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public ICollection<Tasks> Tasks { get; set; } = new List<Tasks>();
}
