using OCS.Domain.Entities;
using OCS.Infrastructure.DataAccess.Repositories.Orders;
using OCS.Infrastructure.Services.Features.Discount;
using OCS.Infrastructure.Services.Features.Orders;
using OCS.Infrastructure.Services.Features.TaxCalculator;

namespace OCS.Infrastructure.Services.Features.OrderCalculator;
public class OrderCalculatorService(IDiscountService discountService,
    ITaxCalculatorService taxCalculatorService,
    IOrderRepository orderRepository) : IOrderCalculatorService
{
    // Assumption hard coding taxes here, these should be coming from configuration or database
    readonly HashSet<string> preDiscountTaxStates = ["FL", "NM", "NV"];

    private async Task<OrderCalculationResult> CalculateOrderAsync(Order order)
    {
        decimal subtotal = CalculateSubtotal(order);
        decimal discount = await discountService.ApplyDiscountsAsync(order, subtotal);

        // for states FL, NM, NV - tax is applied before discount
        // for other states tax is applied after discount
        decimal taxableAmount = preDiscountTaxStates.Contains(order.Client!.State) ? subtotal : subtotal - discount;

        decimal tax = taxCalculatorService.CalculateTax(order.Client!, taxableAmount);

        decimal total = subtotal - discount + tax;

        return new OrderCalculationResult
        {
            Subtotal = subtotal,
            Discount = discount,
            Tax = tax,
            Total = total,
            TaxableAmount = taxableAmount
        };
    }

    private static decimal CalculateSubtotal(Order order)
    {
        return order.OrderItems.Sum(item => item.Quantity * item.Product!.Price);
    }

    public async Task<OrderDto> CalculateOrderTotalsAsync(int orderId)
    {

        var updatedOrder = await orderRepository.GetOrderByIdAsync(orderId) ?? throw new KeyNotFoundException();

        var orderResult = await CalculateOrderAsync(updatedOrder!);

        updatedOrder.TotalAmount = orderResult.Total;
        updatedOrder.TaxAmount = orderResult.Tax;
        updatedOrder.DiscountAmount = orderResult.Discount;

        await orderRepository.UpdateAsync(updatedOrder);

        var orderDto = OrderMapper.ToDto(updatedOrder);

        orderDto.PreTaxAmount = orderResult.TaxableAmount;

        return orderDto;
    }
}
