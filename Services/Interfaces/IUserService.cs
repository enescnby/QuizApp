using System.Linq.Expressions;
using QuizApp.Models;

namespace QuizApp.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(Guid userId);
        Task<IEnumerable<User>> GetAllAsync();
        Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate);
        Task AddAsync(User user);
        Task AddRangeAsync(IEnumerable<User> users);
        Task UpdateAndSave(User user);
        Task UpdateRangeAndSave(IEnumerable<User> users);
        Task DeleteAndSave(User user);
        Task DeleteRangeAndSave(IEnumerable<User> users);

        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsByUsernameAsync(string username);
        Task<bool> IsAdminAsync(Guid userId);
        Task<IEnumerable<Attempt>> GetAttemptsAsync(Guid userId);
        Task<Attempt?> GetLastAttemptAsync(Guid userId);
        Task<IEnumerable<User>> SearchAsync(string keyword);

    }
}