using QuizApp.Data.Repositories.Interfaces;

namespace QuizApp.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        ITestQuestionRepository TestQuestions { get; }
        ICategoryRepository Categories { get; }
        IQuestionReportRepository QuestionReports { get; }

        Task<int> SaveChangesAsync();
    }
}
