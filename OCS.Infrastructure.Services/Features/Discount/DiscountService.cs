using OCS.Domain.Entities;
using OCS.Infrastructure.DataAccess.Repositories.Coupons;
using OCS.Infrastructure.DataAccess.Repositories.Promotions;

namespace OCS.Infrastructure.Services.Features.Discount;
public class DiscountService(ICouponRepository couponRepository, IPromotionRepository promotionRepository) : IDiscountService
{
    public async Task<decimal> ApplyDiscountsAsync(Order order, decimal subtotal)
    {
        decimal discount = 0;

        // Apply coupons - assumption coupon applies to product like Swiggy
        foreach (var item in order.OrderItems)
        {
            var coupon = await couponRepository.GetAsync(x => x.ValidFrom <= order.OrderDate && x.ValidUntil >= order.OrderDate && x.ProductId == item.ProductId);
            if (coupon != null)
            {
                // Implementation of coupon discount calculation
                // Assuming that each product has discount with coupon.
                discount += coupon.DiscountAmount;
            }
        }

        // Apply store-wide promotion - assumption only one promotion per order
        var promotion = await promotionRepository.GetAsync(x => x.ValidFrom <= order.OrderDate && x.ValidUntil >= order.OrderDate);

        if (promotion is not null)
        {
            discount += (subtotal * promotion.DiscountPercentage) / 100;
        }

        return discount;
    }
}
