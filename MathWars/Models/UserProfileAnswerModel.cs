namespace MathWars.Models
{
    public class UserProfileAnswerModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int DifficultyLevel { get; set; }
        public DateTime SubmissionDate {  get; set; } 
    }
}
