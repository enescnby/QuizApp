using QuizApp.Data.Repositories.Interfaces;

namespace QuizApp.Data.UnitOfWork
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IUserRepository Users { get; }
        public ITestQuestionRepository TestQuestions { get; }

        public UnitOfWork(
            ApplicationDbContext context,
            IUserRepository userRepository,
            ITestQuestionRepository testQuestionRepository
        )
        {
            _context = context;
            Users = userRepository;
            TestQuestions = testQuestionRepository;
        }

        public async Task<int> SaveChangesAsync() =>
            await _context.SaveChangesAsync();

        public void Dispose() =>
            _context.Dispose();
    }
}