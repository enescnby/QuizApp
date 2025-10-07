using QuizApp.Models;
using QuizApp.Models.Enums;

namespace QuizApp.Data.Repositories.Interfaces
{
    public interface ITestQuestionRepository : IGenericRepository<TestQuestion>
    {
        Task<IEnumerable<TestQuestion>> GetByCategoryAsync(Category category, int count);
    }
}