using System.Security.Claims;
using QuizApp.Services.Interfaces;

namespace QuizApp.Middlewares
{
    public sealed class TokenExtractionMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenExtractionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string? authHeader = context.Request.Headers["Authorization"];

            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer", StringComparison.OrdinalIgnoreCase))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();
                
                try
                {
                    var tokenService = context.RequestServices.GetRequiredService<ITokenService>();
                    
                    var principal = tokenService.GetClaimsPrincipalFromExpiredToken(token);
                    
                    if (principal != null)
                    {
                        context.Items["JWT"] = token;
                        context.Items["UserId"] = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        context.Items["Username"] = principal.FindFirst(ClaimTypes.Name)?.Value;
                        context.Items["Role"] = principal.FindFirst(ClaimTypes.Role)?.Value;
                        
                        context.User = principal;
                    }
                }
                catch (Exception)
                {
                }
            }

            await _next(context);
        }
    }
}