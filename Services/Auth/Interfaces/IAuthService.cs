using QuizApp.Models;

namespace QuizApp.Services.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(string email, string username, string password);
        Task<(string AccessToken, string RefreshToken)> LoginAsync(string email, string password);
        Task<(string AccessToken, string RefreshToken)> RefreshTokenAsync(string refreshToken);
        Task LogoutAsync(string refreshToken);
    }
}