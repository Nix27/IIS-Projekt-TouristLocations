using DAL.Model;
using System.Linq.Expressions;

namespace DAL.Repository.Abstraction
{
    public interface IRepository<T> where T : DbEntity
    {
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteMultiple(IEnumerable<T> entities);
        Task<T?> GetEntityAsync(Expression<Func<T, bool>> filter, string? includeProperties = null, bool isTracked = true);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, bool isTracked = true);
    }
}
