using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using QuizApp.Data.UnitOfWork;
using QuizApp.Models;
using QuizApp.Services.Interfaces;

namespace QuizApp.Services.Implementations
{
    public sealed class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;

        private static readonly Dictionary<string, Guid> _refreshTokens = new();

        public AuthService
        (
            IUnitOfWork unitOfWork,
            ITokenService tokenService
        )
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        public async Task<User> RegisterAsync(string email, string username, string password)
        {
            if (await _unitOfWork.Users.ExistsByEmailAsync(email))
                throw new Exception("Email is already registered");

            if (await _unitOfWork.Users.ExistsByUsernameAsync(username))
                throw new Exception("Username is taken");

            var hashedPassword = HashPassword(password);

            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = email,
                Username = username,
                PasswordHash = hashedPassword,
                Role = Models.Enums.Role.User,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return user;
        }

        public async Task<(string AccessToken, string RefreshToken)> LoginAsync(string email, string password)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(email);
            if (user == null || !VerifyPassword(password, user.PasswordHash))
                throw new Exception("Invalid Credentials");

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            _refreshTokens[refreshToken] = user.UserId;

            return (accessToken, refreshToken);
        }

        public async Task<(string AccessToken, string RefreshToken)> RefreshTokenAsync(string refreshToken)
        {
            if (!_refreshTokens.TryGetValue(refreshToken, out var userId))
                throw new Exception("Invalid refresh token");

            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            var newAccessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            _refreshTokens.Remove(refreshToken);
            _refreshTokens[newRefreshToken] = user.UserId;

            return (newAccessToken, newRefreshToken);
        }

        public Task LogoutAsync(string refreshToken)
        {
            _refreshTokens.Remove(refreshToken);
            return Task.CompletedTask;
        }

        private static string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);
            byte[] hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32);

            return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        private static bool VerifyPassword(string password, string hashedPassword)
        {
            var parts = hashedPassword.Split('.');
            if (parts.Length != 2)
                return false;

            var salt = Convert.FromBase64String(parts[0]);
            var hash = Convert.FromBase64String(parts[1]);

            var attemptedHash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32
            );

            return hash.SequenceEqual(attemptedHash);
        }
    }
}