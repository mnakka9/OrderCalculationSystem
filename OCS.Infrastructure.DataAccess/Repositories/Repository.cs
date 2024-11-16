using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace OCS.Infrastructure.DataAccess.Repositories;

public class Repository<T>(TaxSystemContext context) : IRepository<T> where T : class
{
    private DbSet<T> EntityDbSet => context.Set<T>();

    public async Task CreateAsync(T entity)
    {
        await EntityDbSet.AddAsync(entity);

        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await EntityDbSet.FindAsync(id) ?? throw new Exception("Entity not found");

        EntityDbSet.Remove(entity);

        await context.SaveChangesAsync();
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await EntityDbSet.AsNoTracking().ToListAsync();
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await EntityDbSet.FirstOrDefaultAsync(predicate);
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await EntityDbSet.FindAsync(id);
    }

    public async Task UpdateAsync(T entity)
    {
        EntityDbSet.Update(entity);

        await context.SaveChangesAsync();
    }
}
