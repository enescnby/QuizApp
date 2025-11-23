using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using QuizApp.Data.UnitOfWork;
using QuizApp.Models;
using QuizApp.Services.Interfaces;

namespace QuizApp.Services.Implementations
{
    public sealed class TestQuestionService : ITestQuestionService
    {

        private readonly IUnitOfWork _unitOfWork;

        public TestQuestionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TestQuestion?> GetByIdAsync(int id) =>
            await _unitOfWork.TestQuestions.GetByIdAsync(id);

        public async Task<IEnumerable<TestQuestion>> GetAllAsync() =>
            await _unitOfWork.TestQuestions.GetAllAsync();

        public async Task<IEnumerable<TestQuestion>> FindAsync(Expression<Func<TestQuestion, bool>> predicate) =>
            await _unitOfWork.TestQuestions.FindAsync(predicate);

        public async Task AddAndSaveAsync(TestQuestion testQuestion)
        {
            await _unitOfWork.TestQuestions.AddAsync(testQuestion);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddRangeAndSaveAsync(IEnumerable<TestQuestion> testQuestions)
        {
            await _unitOfWork.TestQuestions.AddRangeAsync(testQuestions);
            await _unitOfWork.SaveChangesAsync();
        }

        public void Update(TestQuestion testQuestion) =>
            _unitOfWork.TestQuestions.Update(testQuestion);

        public async Task UpdateAndSaveAsync(TestQuestion testQuestion)
        {
            _unitOfWork.TestQuestions.Update(testQuestion);
            await _unitOfWork.SaveChangesAsync();
        }

        public void UpdateRange(IEnumerable<TestQuestion> testQuestions) =>
            _unitOfWork.TestQuestions.UpdateRange(testQuestions);

        public async Task UpdateRangeAndSaveAsync(IEnumerable<TestQuestion> testQuestions)
        {
            _unitOfWork.TestQuestions.UpdateRange(testQuestions);
            await _unitOfWork.SaveChangesAsync();
        }

        public void Delete(TestQuestion testQuestion) =>
            _unitOfWork.TestQuestions.Delete(testQuestion);

        public async Task DeleteAndSaveAsync(TestQuestion testQuestion)
        {
            _unitOfWork.TestQuestions.Delete(testQuestion);
            await _unitOfWork.SaveChangesAsync();
        }

        public void DeleteRange(IEnumerable<TestQuestion> testQuestions) =>
            _unitOfWork.TestQuestions.DeleteRange(testQuestions);

        public async Task DeleteRangeAndSaveAsync(IEnumerable<TestQuestion> testQuestions)
        {
            _unitOfWork.TestQuestions.DeleteRange(testQuestions);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<TestQuestion>> GetByCategoryAsync(string categorySlug, int count)
        {
            if (string.IsNullOrWhiteSpace(categorySlug))
                throw new ArgumentException("category slug can not be null or empty", nameof(categorySlug));

            var normalizedSlug = categorySlug.Trim().ToLowerInvariant();

            var category = await _unitOfWork.Categories.GetBySlugAsync(normalizedSlug)
                ?? throw new KeyNotFoundException("category not found");

            return await _unitOfWork.TestQuestions.GetByCategoryAsync(category.QuestionCategoryId, count);
        }

        public async Task<TestQuestion> CreateAndSaveQuestionAsync(
            string categorySlug,
            string? categoryName,
            string text,
            string optionA,
            string optionB,
            string optionC,
            string optionD,
            string correctOption
        )
        {
            if (string.IsNullOrWhiteSpace(categorySlug))
                throw new ArgumentException("category slug can not be null or empty", nameof(categorySlug));

            var normalizedSlug = categorySlug.Trim().ToLowerInvariant();

            var category = await _unitOfWork.Categories.GetBySlugAsync(normalizedSlug);

            if (category == null)
            {
                var resolvedName = string.IsNullOrWhiteSpace(categoryName)
                    ? normalizedSlug
                    : categoryName.Trim();

                category = new QuestionCategory
                {
                    Name = resolvedName,
                    Slug = normalizedSlug
                };

                await _unitOfWork.Categories.AddAsync(category);
                await _unitOfWork.SaveChangesAsync();
            }

            var options = new[] { "a", "b", "c", "d" };
            var normalizedCorrectOption = correctOption?.Trim().ToLowerInvariant()
                ?? throw new ArgumentException("correct option can not be null or empty", nameof(correctOption));

            if (!options.Contains(normalizedCorrectOption))
                throw new Exception("Correct option must be in [a, b, c, d] ");

            TestQuestion testQuestion = new TestQuestion
            {
                QuestionCategoryId = category.QuestionCategoryId,
                Category = category,
                Text = text,
                OptionA = optionA,
                OptionB = optionB,
                OptionC = optionC,
                OptionD = optionD,
                CorrectOption = normalizedCorrectOption
            };

            await _unitOfWork.TestQuestions.AddAsync(testQuestion);
            await _unitOfWork.SaveChangesAsync();

            return testQuestion;
        }

        public async Task<string> GetCorrectAnswer(int questionId)
        {
            var question = await _unitOfWork.TestQuestions.GetByIdAsync(questionId)
                ?? throw new KeyNotFoundException("question not found");
            return question.CorrectOption;
        }

        public async Task<(bool IsCorrect, string CorrectOption)> GuessTheQuestion(Guid userId, int questionId, string answer)
        {
            var question = await _unitOfWork.TestQuestions.GetByIdAsync(questionId)
                ?? throw new Exception("question not found");
            var user = await _unitOfWork.Users.GetWithStatsAsync(userId)
                ?? throw new Exception("user not found");

            if (user.Stats == null)
            {
                // Create new stats and attach; context is tracking 'user'
                user.Stats = new UserStats { UserId = user.UserId };
            }

            var isCorrect = string.Equals(answer, question.CorrectOption, StringComparison.OrdinalIgnoreCase);

            if (isCorrect)
            {
                user.Stats.SoloCorrectAnswers += 1;
                user.Stats.TotalCorrectAnswers += 1;
                user.Stats.SoloScore += 10;
                user.Stats.TotalScore += 10;
            }
            else
            {
                user.Stats.SoloWrongAnswers += 1;
            }

            await _unitOfWork.SaveChangesAsync();
            return (isCorrect, question.CorrectOption);
        }

        public async Task ReportQuestionAsync(int questionId, Guid? reporterUserId, string? reason)
        {
            var question = await _unitOfWork.TestQuestions.GetByIdAsync(questionId)
                ?? throw new KeyNotFoundException("question not found");

            var normalizedReason = string.IsNullOrWhiteSpace(reason)
                ? null
                : reason.Trim();

            if (normalizedReason is { Length: > 500 })
                throw new ArgumentException("reason can not exceed 500 characters", nameof(reason));

            if (reporterUserId.HasValue)
            {
                var alreadyReported = await _unitOfWork.QuestionReports.ExistsAsync(questionId, reporterUserId.Value);
                if (alreadyReported)
                {
                    return;
                }
            }

            var report = new QuestionReport
            {
                TestQuestionId = questionId,
                ReporterUserId = reporterUserId,
                Reason = normalizedReason
            };

            await _unitOfWork.QuestionReports.AddAsync(report);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<QuestionReport>> GetReportedQuestionsAsync() =>
            await _unitOfWork.QuestionReports.GetAllWithDetailsAsync();

    }
}
