namespace MathWars.Entities;

public class TasksAndCategories
{
    public int TaskId { get; set; }
    public Tasks Task { get; set; } = null!;

    public int TaskCategoryId { get; set; }
    public TasksCategory TaskCategory { get; set; } = null!;
}
