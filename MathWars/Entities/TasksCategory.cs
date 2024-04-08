using System.ComponentModel.DataAnnotations;

namespace MathWars.Entities;

public class TasksCategory
{
    [Key]
    public int Id { get; set; }
    public string CategoryName { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;

    public List<TasksAndCategories> TasksAndCategories { get; set; }
    public List<Tasks> Tasks { get; set; }
}
