using System.Linq.Expressions;

namespace OCS.Infrastructure.DataAccess.Repositories;
public interface IRepository<T> where T : class
{
    Task CreateAsync(T entity);

    Task UpdateAsync(T entity);

    Task DeleteAsync(int id);

    Task<T?> GetByIdAsync(int id);

    Task<T?> GetAsync(Expression<Func<T, bool>> predicate);

    Task<List<T>> GetAllAsync();
}
