using OCS.Domain.Entities;
using OCS.Infrastructure.DataAccess.Repositories.Orders;
using OCS.Infrastructure.DataAccess.Repositories.Products;
using OCS.Infrastructure.Services.Features.OrderCalculator;

namespace OCS.Infrastructure.Services.Features.Orders;

public class OrderService(
    IOrderRepository orderRepository,
    IProductRepository productRepository,
    IOrderCalculatorService calculatorService
) : IOrderService
{
    public async Task<OrderDto> SaveOrderAsync(OrderDto orderDto)
    {
        var order = OrderMapper.Map(orderDto);

        await ValidateOrderItemsAsync(order.OrderItems);

        await orderRepository.CreateAsync(order);

        return await calculatorService.CalculateOrderTotalsAsync(order.Id);
    }

    private async Task ValidateOrderItemsAsync(List<OrderItem> orderItems)
    {
        foreach (var item in orderItems)
        {
            if (await productRepository.GetByIdAsync(item.ProductId) is null)
            {
                throw new KeyNotFoundException(
                    $"One of the order products doesn't present in system"
                );
            }
        }
    }
}
