using Microsoft.EntityFrameworkCore;
using SimpleChat.Data;
using SimpleChat.Models.Abstractions.Repositories;

namespace SimpleChat.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ChatAppContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(ChatAppContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
