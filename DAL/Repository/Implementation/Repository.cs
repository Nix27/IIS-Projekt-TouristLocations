using DAL.Context;
using DAL.Model;
using DAL.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Repository.Implementation
{
    public class Repository<T> : IRepository<T> where T : DbEntity
    {
        private readonly ApplicationContext _context;
        protected readonly DbSet<T> dbSet;

        public Repository(ApplicationContext context)
        {
            _context = context;
            dbSet = context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, bool isTracked = true)
        {
            IQueryable<T> query;

            if (isTracked)
                query = dbSet;
            else
                query = dbSet.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }

            var result = await query.ToListAsync();
            return result.AsEnumerable();
        }

        public async Task<T?> GetEntityAsync(Expression<Func<T, bool>> filter, string? includeProperties = null, bool isTracked = true)
        {
            IQueryable<T> query;

            if (isTracked)
                query = dbSet;
            else
                query = dbSet.AsNoTracking();

            query = query.Where(filter);

            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
        }

        public void DeleteMultiple(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
    }
}
