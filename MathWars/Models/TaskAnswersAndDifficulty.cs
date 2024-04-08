using System.ComponentModel.DataAnnotations;

namespace MathWars.Models
{
    public class TaskAnswersAndDifficulty
    {
        public int NumberOfAnswers { get; set; } = 2;
        public List<int> CorrectAnswers { get; set; } = new List<int>();
        public int DifficultyLevel { get; set; }
        public List<AnswerToTaskModel> AnswersToTaskList { get; set; } = new List<AnswerToTaskModel>();
    }
}
