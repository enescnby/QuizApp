using QuizApp.Models;

namespace QuizApp.Data.Repositories.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByAuth0IdAsync(string auth0Id);
        Task<User?> GetWithStatsAsync(Guid userId);
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsByUsernameAsync(string username);
        Task<bool> IsAdminAsync(Guid userId);
        Task<UserStats> GetStatsAsync(Guid userId);
        Task<IEnumerable<Attempt>> GetAttemptsAsync(Guid userId);
        Task<Attempt?> GetLastAttemptAsync(Guid userId);
        Task<IEnumerable<User>> SearchAsync(string keyword);
    }
}
