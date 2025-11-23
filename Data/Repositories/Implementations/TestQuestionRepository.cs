using Microsoft.EntityFrameworkCore;
using QuizApp.Data.Repositories.Interfaces;
using QuizApp.Models;

namespace QuizApp.Data.Repositories.Implementations
{
    public sealed class TestQuestionRepository : GenericRepository<TestQuestion>, ITestQuestionRepository
    {
        public TestQuestionRepository(ApplicationDbContext context) : base(context) { }

        public async Task<TestQuestion?> GetByIdAsync(int id) =>
            await _dbSet.FindAsync(id);

        public async Task<IEnumerable<TestQuestion>> GetByCategoryAsync(int categoryId, int count) =>
            await _dbSet
                .Where(tq => tq.QuestionCategoryId == categoryId)
                .OrderBy(tq => EF.Functions.Random())
                .Take(count)
                .ToListAsync();
    }
}
