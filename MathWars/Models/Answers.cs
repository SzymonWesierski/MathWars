using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MathWars.Models;

public class Answers
{
    [Key]
    public int Id { get; set; }

    // Klucz obcy do użytkownika
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    // Klucz obcy do zadania
    public int TaskId { get; set; }
    public Tasks Task { get; set; }

    [Required]
    [Range(double.MinValue, double.MaxValue, ErrorMessage = "Please enter a valid number.")]
    public double Answer { get; set; }
    public DateTime SubmissionDate { get; set; } = DateTime.Now;
}
