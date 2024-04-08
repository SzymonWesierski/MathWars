using MathWars.Data;
using MathWars.Entities;
using MathWars.Interfaces;

namespace MathWars.Repository
{
    public class AnswersToTaskRepository : IAnswersToTaskRepository
    {
        private readonly ApplicationDbContext _context;

        public AnswersToTaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAnswerToTaskAsync(AnswersToTask answer)
        {
            await _context.AddAsync(answer);
        }
    }
}
