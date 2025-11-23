using System;
using QuizApp.Models;

namespace QuizApp.Data.Repositories.Interfaces
{
    public interface IQuestionReportRepository
    {
        Task AddAsync(QuestionReport report);
        Task<bool> ExistsAsync(int questionId, Guid reporterUserId);
        Task<IReadOnlyList<QuestionReport>> GetAllWithDetailsAsync();
    }
}
