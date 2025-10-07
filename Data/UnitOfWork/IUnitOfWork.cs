using QuizApp.Data.Repositories.Interfaces;

namespace QuizApp.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        ITestQuestionRepository TestQuestions { get; }

        Task<int> SaveChangesAsync();
    }
}