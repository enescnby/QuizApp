using System;
using Microsoft.EntityFrameworkCore;
using QuizApp.Data.Repositories.Interfaces;
using QuizApp.Models;

namespace QuizApp.Data.Repositories.Implementations
{
    public sealed class CategoryRepository : GenericRepository<QuestionCategory>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context) { }

        public async Task<QuestionCategory?> GetBySlugAsync(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
                throw new ArgumentException("slug can not be null or empty", nameof(slug));

            var normalizedSlug = slug.Trim().ToLowerInvariant();
            return await _dbSet.FirstOrDefaultAsync(c => c.Slug == normalizedSlug);
        }

        public async Task<bool> ExistsBySlugAsync(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
                throw new ArgumentException("slug can not be null or empty", nameof(slug));

            var normalizedSlug = slug.Trim().ToLowerInvariant();
            return await _dbSet.AnyAsync(c => c.Slug == normalizedSlug);
        }
    }
}
