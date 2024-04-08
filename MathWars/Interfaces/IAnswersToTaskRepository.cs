using MathWars.Entities;

namespace MathWars.Interfaces
{
    public interface IAnswersToTaskRepository
    {
        Task CreateAnswerToTaskAsync(AnswersToTask answer);
    }
}
