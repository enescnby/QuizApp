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
                context.Items["JWT"] = token;
            }

            if (context.User?.Identity?.IsAuthenticated == true)
            {
                try
                {
                    var authService = context.RequestServices.GetRequiredService<IAuthService>();
                    var user = await authService.EnsureUserAsync(context.User);

                    context.Items["UserId"] = user.UserId.ToString();
                    context.Items["Username"] = user.Username;
                    context.Items["Role"] = user.Role.ToString();
                }
                catch (Exception)
                {
                }
            }

            await _next(context);
        }
    }
}
