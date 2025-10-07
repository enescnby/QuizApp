using System.Collections;
using System.Linq.Expressions;
using QuizApp.Models;
using QuizApp.Models.Enums;

namespace QuizApp.Services.Interfaces
{
    public interface ITestQuestionService
    {
        Task<TestQuestion?> GetByIdAsync(Guid id);
        Task<IEnumerable<TestQuestion>> GetAllAsync();
        Task<IEnumerable<TestQuestion>> FindAsync(Expression<Func<TestQuestion, bool>> predicate);
        Task AddAndSaveAsync(TestQuestion testQuestion);
        Task AddRangeAndSaveAsync(IEnumerable<TestQuestion> testQuestions);
        void Update(TestQuestion testQuestion);
        Task UpdateAndSaveAsync(TestQuestion testQuestion);
        void UpdateRange(IEnumerable<TestQuestion> testQuestions);
        Task UpdateRangeAndSaveAsync(IEnumerable<TestQuestion> testQuestions);
        void Delete(TestQuestion testQuestion);
        Task DeleteAndSaveAsync(TestQuestion testQuestion);
        void DeleteRange(IEnumerable<TestQuestion> testQuestions);
        Task DeleteRangeAndSaveAsync(IEnumerable<TestQuestion> testQuestions);
        Task<IEnumerable<TestQuestion>> GetByCategoryAsync(Category category, int count);
    }
}