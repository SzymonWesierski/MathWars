using System.ComponentModel.DataAnnotations;

namespace MathWars.Models;

public class Answer
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int UserId { get; set; }
    [Required]
    public int TaskId { get; set; }
    [Required]
    public string Content { get; set; }
    public DateTime SubmissionDate { get; set; }
}