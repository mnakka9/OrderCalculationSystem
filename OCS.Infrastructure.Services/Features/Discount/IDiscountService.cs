using OCS.Domain.Entities;

namespace OCS.Infrastructure.Services.Features.Discount;

public interface IDiscountService
{
    Task<decimal> ApplyDiscountsAsync(Order order, decimal subtotal);
}
