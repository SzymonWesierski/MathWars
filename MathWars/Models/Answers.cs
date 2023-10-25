using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MathWars.Models;

public class Answers
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

    [ForeignKey("TaskId")]
    public Tasks Tasks { get; set; }
    [ForeignKey("UserId")]
    public Users Users { get; set; }
}