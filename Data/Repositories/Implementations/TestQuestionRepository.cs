using Microsoft.EntityFrameworkCore;
using QuizApp.Data.Repositories.Interfaces;
using QuizApp.Models;
using QuizApp.Models.Enums;

namespace QuizApp.Data.Repositories.Implementations
{
    public sealed class TestQuestionRepository : GenericRepository<TestQuestion>, ITestQuestionRepository
    {
        public TestQuestionRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<TestQuestion>> GetByCategory(Category category, int count) =>
            await _dbSet.Where(tq => tq.Category == category)
                .OrderBy(tq => EF.Functions.Random()).Take(count).ToListAsync();
    }
}