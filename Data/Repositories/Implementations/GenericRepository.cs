using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using QuizApp.Data.Repositories.Interfaces;

namespace QuizApp.Data.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        private readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task<T?> GetByIdAsync(Guid id) =>
            await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync() =>
            await _dbSet.ToListAsync();

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) =>
            await _dbSet.Where(predicate).ToListAsync();

        public async Task AddAsync(T entity) =>
            await _dbSet.AddAsync(entity);

        public async Task AddRangeAsync(IEnumerable<T> entities) =>
            await _dbSet.AddRangeAsync(entities);

        public void Delete(T entity) =>
            _dbSet.Remove(entity);

        public void DeleteRange(IEnumerable<T> entities) =>
            _dbSet.RemoveRange(entities);

        public void Update(T entity) =>
            _dbSet.Update(entity);

        public void UpdateRange(IEnumerable<T> entities) =>
            _dbSet.UpdateRange(entities);
    }
}