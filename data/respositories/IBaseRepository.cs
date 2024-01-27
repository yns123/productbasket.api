using System.Linq.Expressions;
using data.entities;

namespace data.respositories;

public interface IBaseRepository<T> where T : IEntity, new()
{
    IQueryable<T> GetList(Expression<Func<T, bool>> predicate, out int count, int page = 1, int limit = 50, bool withCount = false); Task<T> GetAsync(Expression<Func<T, bool>> predicate);
    Task<T> GetByIdAsync(string id);
    Task<T> CreateAsync(T entity);
    Task<bool> BulkCreateAsync(IEnumerable<T> entities);
    Task<T> UpdateAsync(string id, T entity);
    Task<T> UpdateAsync(T entity, Expression<Func<T, bool>> predicate);
    Task<T> DeleteAsync(T entity);
    Task<T> DeleteAsync(string id);
    Task<T> DeleteAsync(Expression<Func<T, bool>> filter);
}
