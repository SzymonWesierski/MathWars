namespace MathWars.Models
{
    public class AnswerToTaskModel
    {
        public int id { get; set; }
        public string Content { get; set; }
        public IFormFile PhotoUrl { get; set; }
        public bool IsCorrect { get; set; } = false;
        public int TaskId { get; set; }
    }
}
