using AutoMapper;
using MathWars.Data;
using MathWars.Interfaces;

namespace MathWars.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UnitOfWork(ApplicationDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public ITaskCategoryRepository TaskCategoryRepository => new TaskCategoryRepository(_context);

        public ITaskRepository TaskRepository => new TaskRepository(_context, _mapper);

        public IUserRepository UserRepository => new UserRepository(_context, _mapper);

        public IUserAnswersRepository UserAnswersRepository => new UserAnswersRepository(_context, _mapper);

		public IUserReportsRepository UserReportsRepository => new UserReportsRepository(_context, _mapper);

		public ITaskRatingRepository TaskRatingRepository => new TaskRatingRepository(_context);

		public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            _context.ChangeTracker.DetectChanges();
            var changes = _context.ChangeTracker.HasChanges();

            return changes;
        }
    }
}
