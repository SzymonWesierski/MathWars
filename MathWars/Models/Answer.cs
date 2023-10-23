namespace MathWars.Models;

public class Answer
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int TaskId { get; set; }
    public string Content { get; set; }
    public DateTime SubmissionDate { get; set; }
}