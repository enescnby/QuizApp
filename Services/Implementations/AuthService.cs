using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using QuizApp.Data.UnitOfWork;
using QuizApp.Models;
using QuizApp.Services.Interfaces;

namespace QuizApp.Services.Implementations
{
    public sealed class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly HashSet<string> _adminPermissions;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            var configuredPermissions = configuration
                .GetSection("Auth0:AdminPermissions")
                .Get<string[]>();

            _adminPermissions = configuredPermissions != null
                ? new HashSet<string>(
                    configuredPermissions.Where(p => !string.IsNullOrWhiteSpace(p)),
                    StringComparer.OrdinalIgnoreCase)
                : new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        public async Task<User> EnsureUserAsync(ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            var auth0Id = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? principal.FindFirst("sub")?.Value;

            if (string.IsNullOrWhiteSpace(auth0Id))
                throw new InvalidOperationException("Authenticated principal does not include a subject identifier.");

            var existingUser = await _unitOfWork.Users.GetByAuth0IdAsync(auth0Id);
            var email = principal.FindFirst("https://qioapp.com/email")?.Value
                ?? principal.FindFirst(ClaimTypes.Email)?.Value;
            var preferredUsername = principal.FindFirst("https://qioapp.com/username")?.Value
                ?? principal.FindFirst("nickname")?.Value
                ?? principal.FindFirst(ClaimTypes.Name)?.Value
                ?? email
                ?? auth0Id;
            var claimedRole = ResolveRole(principal);

            if (existingUser != null)
            {
                var requiresUpdate = false;

                if (!string.IsNullOrWhiteSpace(email) &&
                    !string.Equals(existingUser.Email, email, StringComparison.OrdinalIgnoreCase))
                {
                    existingUser.Email = email;
                    requiresUpdate = true;
                }

                if (!string.IsNullOrWhiteSpace(preferredUsername) &&
                    !string.Equals(existingUser.Username, preferredUsername, StringComparison.Ordinal))
                {
                    existingUser.Username = preferredUsername;
                    requiresUpdate = true;
                }

                if (existingUser.Role != claimedRole)
                {
                    existingUser.Role = claimedRole;
                    requiresUpdate = true;
                }

                if (requiresUpdate)
                {
                    _unitOfWork.Users.Update(existingUser);
                    await _unitOfWork.SaveChangesAsync();
                }

                return existingUser;
            }

            var newUserId = Guid.NewGuid();
            var user = new User
            {
                UserId = newUserId,
                Auth0Id = auth0Id,
                Email = email ?? string.Empty,
                Username = preferredUsername,
                Role = claimedRole,
                CreatedAt = DateTime.UtcNow,
                Stats = new UserStats
                {
                    UserStatsId = Guid.NewGuid(),
                    UserId = newUserId
                }
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return user;
        }

        public Task<User?> GetByAuth0IdAsync(string auth0Id)
        {
            if (string.IsNullOrWhiteSpace(auth0Id))
                throw new ArgumentException("Auth0 identifier can not be null or empty.", nameof(auth0Id));

            return _unitOfWork.Users.GetByAuth0IdAsync(auth0Id);
        }

        private Models.Enums.Role ResolveRole(ClaimsPrincipal principal)
        {
            var roleClaimTypes = new[]
            {
                ClaimTypes.Role,
                "role",
                "roles",
                "https://qioapp.com/role",
                "https://qioapp.com/roles"
            };

            foreach (var claimType in roleClaimTypes)
            {
                var raw = principal.FindFirst(claimType)?.Value;
                if (string.IsNullOrWhiteSpace(raw))
                    continue;

                if (Enum.TryParse<Models.Enums.Role>(raw, true, out var parsedRole))
                    return parsedRole;
            }

            var permissionClaims = principal.Claims
                .Where(c => string.Equals(c.Type, "permissions", StringComparison.OrdinalIgnoreCase))
                .Select(c => c.Value);

            if (_adminPermissions.Count > 0 &&
                permissionClaims.Any(permission => _adminPermissions.Contains(permission)))
            {
                return Models.Enums.Role.Admin;
            }

            if (permissionClaims.Any(permission =>
                    permission.Contains("admin", StringComparison.OrdinalIgnoreCase)))
            {
                return Models.Enums.Role.Admin;
            }

            return Models.Enums.Role.User;
        }
    }
}
