using System.Security.Claims;
using QuizApp.Models;

namespace QuizApp.Services.Auth.Implementations
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetClaimsPrincipalFromExpiredToken(string token);
    }
}