using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.DTOs;
using QuizApp.Models.Enums;
using QuizApp.Services.Interfaces;

namespace QuizApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class QuestionController : BaseController
    {
        private readonly ITestQuestionService _testQuestionService;

        public QuestionController(ITestQuestionService testQuestionService)
        {
            _testQuestionService = testQuestionService;
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetByCategory([FromRoute] Category category, [FromQuery] int count = 10)
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
        [HttpPost("admin/createTestQuestion")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadNewTestQuestion([FromBody] UploadTestQuestion request)
        {
            try
            {
                var userRole = GetUserRole();
                var userId = GetUserIdAsGuid();
                var username = GetUsername();

                var question = await _testQuestionService.CreateAndSaveQuestionAsync(
                    request.Category,
                    request.Text,
                    request.OptionA,
                    request.OptionB,
                    request.OptionC,
                    request.OptionD,
                    request.CorrectOption
                );
                return Ok(question);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteQuestion(Guid id)
        {
            try
            {
                var username = GetUsername();

                var question = await _testQuestionService.GetByIdAsync(id);
                if (question == null)
                {
                    return NotFound(new { message = "Question not found" });
                }

                await _testQuestionService.DeleteAndSaveAsync(question);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("admin/all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllQuestionsForAdmin()
        {
            try
            {
                var questions = await _testQuestionService.GetAllAsync();
                return Ok(questions);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("answer/{questionId}")]
        [Authorize]
        public async Task<IActionResult> GetCorrectAnswer([FromRoute] Guid questionId)
        {
            var result = await _testQuestionService.GetCorrectAnswer(questionId);
            return Ok(result);
        }

        [HttpPost("guess/{questionId}/{answer}")]
        [Authorize]
        public async Task<IActionResult> GuessTheQuestion([FromRoute] Guid questionId, [FromRoute] string answer)
        {
            var userId = GetUserIdAsGuid() ?? throw new Exception("invalid id");

            var result = await _testQuestionService.GuessTheQuestion(userId, questionId, answer);

            return Ok(result);
        }
    }
}