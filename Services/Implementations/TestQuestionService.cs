using System.Linq.Expressions;
using QuizApp.Data.UnitOfWork;
using QuizApp.Models;
using QuizApp.Models.Enums;
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

        public async Task<TestQuestion?> GetByIdAsync(Guid id) =>
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

        public async Task<IEnumerable<TestQuestion>> GetByCategoryAsync(Category category, int count) =>
            await _unitOfWork.TestQuestions.GetByCategoryAsync(category, count);

        public async Task<TestQuestion> CreateAndSaveQuestionAsync(
            Category category,
            string text,
            string optionA,
            string optionB,
            string optionC,
            string optionD,
            string correctOption
        )
        {
            var x = new List<string>();
            x.AddRange("a", "b", "c", "d");

            if (!x.Contains(correctOption.ToLower()))
                throw new Exception("Correct option must be in [a, b, c, d] ");

            TestQuestion testQuestion = new TestQuestion
            {
                TestQuestionId = Guid.NewGuid(),
                Category = category,
                Text = text,
                OptionA = optionA,
                OptionB = optionB,
                OptionC = optionC,
                OptionD = optionD,
                CorrectOption = correctOption
            };

            await _unitOfWork.TestQuestions.AddAsync(testQuestion);
            await _unitOfWork.SaveChangesAsync();

            return testQuestion;
        }

        public async Task<string> GetCorrectAnswer(Guid questionId)
        {
            var question = await _unitOfWork.TestQuestions.GetByIdAsync(questionId)
                ?? throw new KeyNotFoundException("question not found");
            return question.CorrectOption;
        }

        public async Task<bool> GuessTheQuestion(Guid userId, Guid questionId, string answer)
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

            if (string.Equals(answer, question.CorrectOption, StringComparison.OrdinalIgnoreCase))
            {
                user.Stats.SoloCorrectAnswers += 1;
                user.Stats.TotalCorrectAnswers += 1;
                user.Stats.SoloScore += 10;
                user.Stats.TotalScore += 10;
                await _unitOfWork.SaveChangesAsync();
                return true;
            } else
            {
                user.Stats.SoloWrongAnswers += 1;
                await _unitOfWork.SaveChangesAsync();
                return false;
            }

        }

    }
}