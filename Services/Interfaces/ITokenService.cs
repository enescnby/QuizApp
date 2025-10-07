using System.Security.Claims;
using QuizApp.Models;

namespace QuizApp.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetClaimsPrincipalFromExpiredToken(string token);
    }
}