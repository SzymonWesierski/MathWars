using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MathWars.Models;

public class Answers
{
    [Key]
    public int Id { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    // Foreign Key to Users
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    // Foreign Key to Tasks
    public int TaskId { get; set; }
    public Tasks Task { get; set; }

    [Required]
    [Range(double.MinValue, double.MaxValue, ErrorMessage = "Please enter a valid number.")]
    public string Answer { get; set; }
    public DateTime SubmissionDate { get; set; } = DateTime.Now;
}
