using Microsoft.EntityFrameworkCore;
using OCS.Domain.Entities;

namespace OCS.Infrastructure.DataAccess.Repositories.Orders;
public class OrderRepository(TaxSystemContext context) : Repository<Order>(context), IOrderRepository
{
    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        return await context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(o => o.Product)
            .Include(o => o.Client)
            .AsSplitQuery()
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<List<Order>> GetOrdersByClientIdAsync(int clientId)
    {
        return await context.Orders
            .Where(o => o.ClientId == clientId)
            .ToListAsync();
    }
}
