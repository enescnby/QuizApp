using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuizApp.Data.Repositories.Interfaces;
using QuizApp.Models;

namespace QuizApp.Data.Repositories.Implementations
{
    public sealed class QuestionReportRepository : IQuestionReportRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<QuestionReport> _dbSet;

        public QuestionReportRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<QuestionReport>();
        }

        public async Task AddAsync(QuestionReport report) =>
            await _dbSet.AddAsync(report);

        public async Task<bool> ExistsAsync(int questionId, Guid reporterUserId) =>
            await _dbSet.AnyAsync(r =>
                r.TestQuestionId == questionId &&
                r.ReporterUserId == reporterUserId);

        public async Task<IReadOnlyList<QuestionReport>> GetAllWithDetailsAsync() =>
            await _dbSet
                .Include(r => r.Question)
                    .ThenInclude(q => q.Category)
                .Include(r => r.Reporter)
                .OrderByDescending(r => r.ReportedAt)
                .ToListAsync();
    }
}
