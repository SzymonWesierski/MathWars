namespace MathWars.Interfaces
{
    public interface IUnitOfWork
    {
        ITaskCategoryRepository TaskCategoryRepository { get; }
        ITaskRepository TaskRepository { get; }
        IUserRepository UserRepository { get; }
        IUserAnswersRepository UserAnswersRepository { get; }
		IUserReportsRepository UserReportsRepository { get; }
		Task<bool> Complete();
        bool HasChanges();
    }
}
