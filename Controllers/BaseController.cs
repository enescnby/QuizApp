using Microsoft.AspNetCore.Mvc;

namespace QuizApp.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected string? GetToken()
        {
            return HttpContext.Items["JWT"] as string;
        }

        protected string? GetUserId()
        {
            return HttpContext.Items["UserId"] as string;
        }

        protected string? GetUsername()
        {
            return HttpContext.Items["Username"] as string;
        }

        protected string? GetUserRole()
        {
            return HttpContext.Items["Role"] as string;
        }

        protected Guid? GetUserIdAsGuid()
        {
            var userIdString = GetUserId();
            if (Guid.TryParse(userIdString, out var userId))
            {
                return userId;
            }
            return null;
        }
    }
}