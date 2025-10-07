using Microsoft.AspNetCore.Mvc;
using QuizApp.Models.Enums;
using QuizApp.Services.Interfaces;

namespace QuizApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class QuestionController : ControllerBase
    {
        private readonly ITestQuestionService _testQuestionService;

        public QuestionController(ITestQuestionService testQuestionService)
        {
            _testQuestionService = testQuestionService;
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetByCategory([FromQuery] Category category, [FromQuery] int count = 10)
        {
            try
            {
                var questions = await _testQuestionService.GetByCategoryAsync(category, count);
                return Ok(questions);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}