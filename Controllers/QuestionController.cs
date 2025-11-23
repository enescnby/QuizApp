using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.DTOs;
using QuizApp.Models;
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

        [HttpGet("category/{categorySlug}")]
        public async Task<IActionResult> GetByCategory([FromRoute] string categorySlug, [FromQuery] int count = 10)
        {
            try
            {
                var questions = await _testQuestionService.GetByCategoryAsync(categorySlug, count);
                var response = questions.Select(q => new QuestionResponse
                {
                    TestQuestionId = q.TestQuestionId,
                    QuestionCategoryId = q.QuestionCategoryId,
                    Text = q.Text,
                    OptionA = q.OptionA,
                    OptionB = q.OptionB,
                    OptionC = q.OptionC,
                    OptionD = q.OptionD
                });
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("admin/createTestQuestions")]
        [Authorize(Policy = "ManageQuestions")]
        public async Task<IActionResult> UploadNewTestQuestions([FromBody] IEnumerable<UploadTestQuestion> requests)
        {
            try
            {
                if (requests == null)
                    return BadRequest(new { message = "Request body cannot be null." });

                var requestList = requests.ToList();
                if (requestList.Count == 0)
                    return BadRequest(new { message = "At least one question must be provided." });

                var createdQuestions = new List<TestQuestion>(requestList.Count);

                foreach (var request in requestList)
                {
                    if (string.IsNullOrWhiteSpace(request.CategorySlug))
                        return BadRequest(new { message = "CategorySlug can not be null or empty." });

                    var question = await _testQuestionService.CreateAndSaveQuestionAsync(
                        request.CategorySlug,
                        request.CategoryName,
                        request.Text,
                        request.OptionA,
                        request.OptionB,
                        request.OptionC,
                        request.OptionD,
                        request.CorrectOption
                    );

                    createdQuestions.Add(question);
                }

                return Ok(createdQuestions);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("admin/createTestQuestion")]
        [Authorize(Policy = "ManageQuestions")]
        public async Task<IActionResult> UploadNewTestQuestion([FromBody] UploadTestQuestion request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.CategorySlug))
                    return BadRequest(new { message = "CategorySlug can not be null or empty." });

                var question = await _testQuestionService.CreateAndSaveQuestionAsync(
                    request.CategorySlug,
                    request.CategoryName,
                    request.Text,
                    request.OptionA,
                    request.OptionB,
                    request.OptionC,
                    request.OptionD,
                    request.CorrectOption
                );
                return Ok(question);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "ManageQuestions")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            try
            {
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
        [Authorize(Policy = "ManageQuestions")]
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

        [HttpGet("admin/reports")]
        [Authorize(Policy = "ManageQuestions")]
        public async Task<IActionResult> GetReportedQuestionsForAdmin()
        {
            try
            {
                var reports = await _testQuestionService.GetReportedQuestionsAsync();
                var response = reports.Select(report => new QuestionReportResponse
                {
                    QuestionReportId = report.QuestionReportId,
                    TestQuestionId = report.TestQuestionId,
                    QuestionText = report.Question.Text,
                    CategoryName = report.Question.Category.Name,
                    CategorySlug = report.Question.Category.Slug,
                    OptionA = report.Question.OptionA,
                    OptionB = report.Question.OptionB,
                    OptionC = report.Question.OptionC,
                    OptionD = report.Question.OptionD,
                    CorrectOption = report.Question.CorrectOption,
                    Reason = report.Reason,
                    ReporterUserId = report.ReporterUserId,
                    ReporterUsername = report.Reporter?.Username,
                    ReportedAt = report.ReportedAt
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("answer/{questionId}")]
        [Authorize]
        public async Task<IActionResult> GetCorrectAnswer([FromRoute] int questionId)
        {
            var result = await _testQuestionService.GetCorrectAnswer(questionId);
            return Ok(result);
        }

        [HttpPost("guess/{questionId}/{answer}")]
        [Authorize]
        public async Task<IActionResult> GuessTheQuestion([FromRoute] int questionId, [FromRoute] string answer)
        {
            var userId = GetUserIdAsGuid() ?? throw new Exception("invalid id");

            var (isCorrect, correctOption) = await _testQuestionService.GuessTheQuestion(userId, questionId, answer);

            var response = new GuessQuestionResponse
            {
                IsCorrect = isCorrect,
                CorrectOption = correctOption
            };

            return Ok(response);
        }

        [HttpPost("{questionId}/report")]
        [Authorize]
        public async Task<IActionResult> ReportQuestion([FromRoute] int questionId, [FromBody] ReportQuestionRequest? request)
        {
            try
            {
                var userId = GetUserIdAsGuid();
                var reason = request?.Reason;

                await _testQuestionService.ReportQuestionAsync(questionId, userId, reason);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
