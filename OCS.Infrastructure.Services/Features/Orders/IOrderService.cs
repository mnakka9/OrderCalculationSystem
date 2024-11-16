namespace OCS.Infrastructure.Services.Features.Orders;
public interface IOrderService
{
    Task<OrderDto> SaveOrderAsync(OrderDto orderDto);
}
