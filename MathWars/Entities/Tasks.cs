using System.ComponentModel.DataAnnotations;

namespace MathWars.Entities;

public class Tasks
{
    [Key]
    public int Id { get; set; }
    public string Content { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string PublicImageId { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
	public int DifficultyLevel { get; set; }
    public int NumberOfCorrectAnswers { get; set; }
	public List<TasksAndCategories> TasksAndCategories { get; set; }
    public List<TasksCategory> Category { get; set; }

    public List<AnswersToTask> AnswersToTask { get; set; }

    public List<UserAnswer> Answers { get; set; } = new();
}
