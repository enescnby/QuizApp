using System.Security.Claims;
using QuizApp.Models;

namespace QuizApp.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User> EnsureUserAsync(ClaimsPrincipal principal);
        Task<User?> GetByAuth0IdAsync(string auth0Id);
    }
}
