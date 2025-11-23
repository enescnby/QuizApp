using QuizApp.Models;

namespace QuizApp.Data.Repositories.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<QuestionCategory>
    {
        Task<QuestionCategory?> GetBySlugAsync(string slug);
        Task<bool> ExistsBySlugAsync(string slug);
    }
}
