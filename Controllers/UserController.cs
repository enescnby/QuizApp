using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.DTOs;
using QuizApp.Services.Interfaces;

namespace QuizApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("stats")]
        [Authorize]
        public async Task<IActionResult> GetStats()
        {
            try
            {
                var userId = GetUserIdAsGuid();
                if (!userId.HasValue)
                    return Unauthorized(new { message = "user id could not be resolved from token" });

                var stats = await _userService.GetStatsAsync(userId.Value);

                var response = new UserStatsResponse
                {
                    UserId = stats.UserId,
                    TotalScore = stats.TotalScore,
                    SoloScore = stats.SoloScore,
                    SoloCorrectAnswers = stats.SoloCorrectAnswers,
                    SoloWrongAnswers = stats.SoloWrongAnswers,
                    DuelScore = stats.DuelScore,
                    DuelCorrectAnswers = stats.DuelCorrectAnswers,
                    DuelWrongAnswers = stats.DuelWrongAnswes,
                    TotalQuizzesPlayed = stats.TotalQuizzesPlayed,
                    TotalCorrectAnswers = stats.TotalCorrectAnswers
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
