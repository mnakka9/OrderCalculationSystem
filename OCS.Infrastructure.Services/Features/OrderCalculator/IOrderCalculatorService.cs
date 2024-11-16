using OCS.Infrastructure.Services.Features.Orders;

namespace OCS.Infrastructure.Services.Features.OrderCalculator;
public interface IOrderCalculatorService
{
    Task<OrderDto> CalculateOrderTotalsAsync(int orderId);
}
