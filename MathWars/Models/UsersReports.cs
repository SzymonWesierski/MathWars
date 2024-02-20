using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MathWars.Models;

public class UsersReports
{
    [Key]
    public int Id { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    [Required(ErrorMessage = "Proszę podać tytuł"), Display(Name = "Tytuł:")]
    public string Title { get; set; }
    [Required(ErrorMessage = "Proszę podać opis problemu"), Display(Name = "Opis:")]
    public string Description { get; set; }
    [Display(Name = "Możesz dodać zdjęcie:")]
    public string? ImagePath { get; set; }

    public string UserId { get; set; } = string.Empty;
    public int? TaskId { get; set; }
}
