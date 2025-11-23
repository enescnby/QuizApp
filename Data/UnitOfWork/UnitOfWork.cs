using QuizApp.Data.Repositories.Interfaces;

namespace QuizApp.Data.UnitOfWork
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IUserRepository Users { get; }
        public ITestQuestionRepository TestQuestions { get; }
        public ICategoryRepository Categories { get; }
        public IQuestionReportRepository QuestionReports { get; }

        public UnitOfWork(
            ApplicationDbContext context,
            IUserRepository userRepository,
            ITestQuestionRepository testQuestionRepository,
            ICategoryRepository categoryRepository,
            IQuestionReportRepository questionReportRepository
        )
        {
            _context = context;
            Users = userRepository;
            TestQuestions = testQuestionRepository;
            Categories = categoryRepository;
            QuestionReports = questionReportRepository;
        }

        public async Task<int> SaveChangesAsync() =>
            await _context.SaveChangesAsync();

        public void Dispose() =>
            _context.Dispose();
    }
}
