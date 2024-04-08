using MathWars.Entities;

namespace MathWars.Models
{
	public class TaskSolvingModel
	{
		public int Id { get; set; }
		public string Content { get; set; }
		public string ImageUrl { get; set; } = string.Empty;
		public int DifficultyLevel { get; set; }
        public int NumberOfCorrectAnswers { get; set; }
        public List<AnswersToTask> AnswersToTask { get; set; }
	}
}
