using OCS.Domain.Entities;

namespace OCS.Infrastructure.DataAccess.Repositories.Orders;
public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetOrderByIdAsync(int id);
    Task<List<Order>> GetOrdersByClientIdAsync(int clientId);
}
