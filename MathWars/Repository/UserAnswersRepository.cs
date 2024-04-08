using AutoMapper;
using AutoMapper.QueryableExtensions;
using MathWars.Data;
using MathWars.Entities;
using MathWars.Helpers;
using MathWars.Interfaces;
using MathWars.Models;
using Microsoft.EntityFrameworkCore;

namespace MathWars.Repository
{
	public class UserAnswersRepository : IUserAnswersRepository
	{
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public UserAnswersRepository(ApplicationDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<UserAnswer> GetUserAnswersAndTaskAsync(int id)
		{
			return await _context.Answers.Include(a => a.Task)
				.FirstOrDefaultAsync(a => a.Id == id);
		}

		public IQueryable<UserAnswer> DidUserSolvedTask(int taskId, string userId)
		{
			return _context.Answers.Where(t => t.TaskId == taskId && t.UserId == userId);
		}

		public async Task CreateUserAnswer(UserAnswer userAnswer)
		{
			await _context.Answers.AddAsync(userAnswer);
		}

		public async Task<PaginatedList<UserProfileAnswerModel>> GetUserProfileAnswersAsync(
			int pageSize, int? pageIndex, string userId)
		{
			var answerQuery = _context.Answers
				.Include(a => a.Task)
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.SubmissionDate)
                .AsNoTracking()
                .ProjectTo<UserProfileAnswerModel>(_mapper.ConfigurationProvider);


            return await PaginatedList<UserProfileAnswerModel>.CreateAsync(answerQuery, pageIndex ?? 1, pageSize);
        }
	}
}
