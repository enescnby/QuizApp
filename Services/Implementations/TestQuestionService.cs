using System.Linq.Expressions;
using System.Threading.Tasks;
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
    }
}