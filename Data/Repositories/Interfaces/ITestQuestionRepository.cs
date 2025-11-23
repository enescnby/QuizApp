using QuizApp.Models;

namespace QuizApp.Data.Repositories.Interfaces
{
    public interface ITestQuestionRepository : IGenericRepository<TestQuestion>
    {
        Task<TestQuestion?> GetByIdAsync(int id);
        Task<IEnumerable<TestQuestion>> GetByCategoryAsync(int categoryId, int count);
    }
}
