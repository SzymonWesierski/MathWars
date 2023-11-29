using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MathWars.Models;

public class TasksCategory
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string CategoryName { get; set; }
    public ICollection<Tasks> Tasks { get; set; } = new List<Tasks>();
}
