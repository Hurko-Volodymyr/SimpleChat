
using System.Linq.Expressions;

namespace SimpleChat.Models.Abstractions.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task SaveChangesAsync();
        Task LoadCollectionAsync<U>(T entity, Expression<Func<T, IEnumerable<U>>> collectionExpression) where U : class;
        Task LoadReferenceAsync<U>(T entity, Expression<Func<T, U>> referenceExpression) where U : class;
    }
}
