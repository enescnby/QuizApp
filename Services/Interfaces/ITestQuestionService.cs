using System.Collections;
using System.Linq.Expressions;
using QuizApp.Models;

namespace QuizApp.Services.Interfaces
{
    public interface ITestQuestionService
    {
        Task<TestQuestion?> GetByIdAsync(int id);
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
        Task<IEnumerable<TestQuestion>> GetByCategoryAsync(string categorySlug, int count);
        Task<TestQuestion> CreateAndSaveQuestionAsync(
            string categorySlug,
            string? categoryName,
            string text,
            string optionA,
            string optionB,
            string optionC,
            string optionD,
            string correctOption
        );

        Task<string> GetCorrectAnswer(int questionId);
        Task<(bool IsCorrect, string CorrectOption)> GuessTheQuestion(Guid userId, int questionId, string answer);
        Task ReportQuestionAsync(int questionId, Guid? reporterUserId, string? reason);
        Task<IEnumerable<QuestionReport>> GetReportedQuestionsAsync();
    }
}
