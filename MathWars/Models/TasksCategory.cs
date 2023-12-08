using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MathWars.Models;

public class TasksCategory
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string CategoryName { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;

	// Foreign Key to Tasks
	public List<TasksAndCategories> TasksAndCategories { get; set; } = new();
	public List<Tasks> Tasks { get; set; } = new();
}
