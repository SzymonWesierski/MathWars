using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MathWars.Models;

public class Answers
{
    [Key]
    public int Id { get; set; }
    // Klucz obcy do użytkownika
    public int UserId { get; set; }
    public Users User { get; set; }

    // Klucz obcy do zadania
    public int TaskId { get; set; }
    public Tasks Task { get; set; }


    [Required]
    public string Content { get; set; }
    public DateTime SubmissionDate { get; set; }
}