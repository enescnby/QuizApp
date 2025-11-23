using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QuizApp.Data.Repositories.Interfaces;
using QuizApp.Models;
using QuizApp.Models.Enums;

namespace QuizApp.Data.Repositories.Implementations
{
    public sealed class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }

        public async Task<User?> GetByEmailAsync(string email) =>
            await _dbSet.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<User?> GetByUsernameAsync(string username) =>
            await _dbSet.FirstOrDefaultAsync(u => u.Username == username);

        public async Task<User?> GetByAuth0IdAsync(string auth0Id) =>
            await _dbSet.FirstOrDefaultAsync(u => u.Auth0Id == auth0Id);

        public async Task<bool> ExistsByEmailAsync(string email) =>
            await _dbSet.AnyAsync(u => u.Email == email);

        public async Task<bool> ExistsByUsernameAsync(string username) =>
            await _dbSet.AnyAsync(u => u.Username == username);

        public async Task<User?> GetWithStatsAsync(Guid userId)
        {
            return await _dbSet
                .Include(u => u.Stats)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<bool> IsAdminAsync(Guid userId)
        {
            User user = await _dbSet.FindAsync(userId)
                ?? throw new Exception("user not found");

            return user.Role == Role.Admin;
        }

        public async Task<UserStats> GetStatsAsync(Guid userId)
        {
            var user = await _dbSet
                .Include(u => u.Stats)
                .FirstOrDefaultAsync(u => u.UserId == userId)
                ?? throw new Exception("user not found");

            if (user.Stats == null)
                throw new Exception("user stats not found");

            return user.Stats;
        }

        public async Task<IEnumerable<Attempt>> GetAttemptsAsync(Guid userId)
        {
            User user = await _dbSet.FindAsync(userId)
                ?? throw new Exception("user not found");

            return user.Attempts;
        }

        public async Task<Attempt?> GetLastAttemptAsync(Guid userId)
        {
            return await _dbSet
                .Where(u => u.UserId == userId)
                .SelectMany(u => u.Attempts)
                .OrderByDescending(a => a.FinishedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> SearchAsync(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                throw new Exception("keyword can not be null or empty");

            keyword = keyword.ToLower();

            return await _dbSet
                .Where(u => u.Username.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                    || u.Email.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                ).ToListAsync();
        }

    }
}
