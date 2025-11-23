using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using QuizApp.Data.UnitOfWork;
using QuizApp.Models;
using QuizApp.Services.Interfaces;

namespace QuizApp.Services.Implementations
{
    public sealed class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User?> GetUserByIdAsync(Guid userId) =>
            await _unitOfWork.Users.GetByIdAsync(userId);

        public async Task<IEnumerable<User>> GetAllAsync() =>
            await _unitOfWork.Users.GetAllAsync();

        public async Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate) =>
            await _unitOfWork.Users.FindAsync(predicate);

        public async Task AddAsync(User user) =>
            await _unitOfWork.Users.AddAsync(user);

        public async Task AddRangeAsync(IEnumerable<User> users) =>
            await _unitOfWork.Users.AddRangeAsync(users);

        public async Task UpdateAndSave(User user)
        {
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateRangeAndSave(IEnumerable<User> users)
        {
            _unitOfWork.Users.UpdateRange(users);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAndSave(User user)
        {
            _unitOfWork.Users.Delete(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteRangeAndSave(IEnumerable<User> users)
        {
            _unitOfWork.Users.DeleteRange(users);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<User?> GetByEmailAsync(string email) =>
            await _unitOfWork.Users.GetByEmailAsync(email);

        public async Task<User?> GetByUsernameAsync(string username) =>
            await _unitOfWork.Users.GetByUsernameAsync(username);

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(email);
            if (user == null)
                return false;
            return true;
        }

        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            var user = await _unitOfWork.Users.GetByUsernameAsync(username);
            if (user == null)
                return false;
            return true;
        }

        public async Task<bool> IsAdminAsync(Guid userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null || user.Role != Models.Enums.Role.Admin)
                return false;
            return true;
        }

        public async Task<IEnumerable<Attempt>> GetAttemptsAsync(Guid userId) =>
            await _unitOfWork.Users.GetAttemptsAsync(userId);

        public async Task<Attempt?> GetLastAttemptAsync(Guid userId) =>
            await _unitOfWork.Users.GetLastAttemptAsync(userId);

        public async Task<IEnumerable<User>> SearchAsync(string keyword) =>
            await _unitOfWork.Users.SearchAsync(keyword);

        public async Task<UserStats> GetStatsAsync(Guid userId) =>
            await _unitOfWork.Users.GetStatsAsync(userId);
    }
}
