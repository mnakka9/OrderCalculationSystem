using Moq;
using OCS.Domain.Entities;
using OCS.Infrastructure.DataAccess.Repositories.Coupons;
using OCS.Infrastructure.DataAccess.Repositories.Promotions;
using OCS.Infrastructure.Services.Features.Discount;
using System.Linq.Expressions;

namespace OCS.Infra.Tests.Discount;

[TestClass]
public class DiscountServiceTests
{
    private Mock<ICouponRepository> _couponRepositoryMock = default!;
    private Mock<IPromotionRepository> _promotionRepositoryMock = default!;
    private DiscountService _discountService = default!;

    [TestInitialize]
    public void Setup()
    {
        _couponRepositoryMock = new Mock<ICouponRepository>();
        _promotionRepositoryMock = new Mock<IPromotionRepository>();
        _discountService = new DiscountService(_couponRepositoryMock.Object, _promotionRepositoryMock.Object);
    }

    [TestMethod]
    public async Task ApplyDiscountsAsync_WithValidCouponsAndPromotion_ReturnsCorrectDiscount()
    {
        // Arrange
        var order = new Order
        {
            OrderDate = DateTime.Now,
            OrderItems =
            [
                new OrderItem { ProductId = 1 },
                new OrderItem { ProductId = 2 }
            ]
        };
        decimal subtotal = 200m;
        decimal expectedDiscount = 30m;

        // Assumption coupon is applier on each product so discount is 20
        var coupon = new Coupon { Code = "TT", DiscountAmount = 10m, ValidFrom = DateTime.Now.AddDays(-1), ValidUntil = DateTime.Now.AddDays(1) };
        _couponRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Coupon, bool>>>()))
                              .ReturnsAsync(coupon);

        //Promotion applied on order with percentage so discount is 10
        var promotion = new Promotion { Name = "Active", DiscountPercentage = 5m, ValidFrom = DateTime.Now.AddDays(-1), ValidUntil = DateTime.Now.AddDays(1) };
        _promotionRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Promotion, bool>>>()))
                                 .ReturnsAsync(promotion);

        // Act
        var result = await _discountService.ApplyDiscountsAsync(order, subtotal);

        // Assert
        Assert.AreEqual(expectedDiscount, result);
    }

    [TestMethod]
    public async Task ApplyDiscountsAsync_WithNoCouponsAndPromotion_ReturnsZeroDiscount()
    {
        // Arrange
        var order = new Order
        {
            OrderDate = DateTime.Now,
            OrderItems =
            [
                new OrderItem { ProductId = 1 },
                new OrderItem { ProductId = 2 }
            ]
        };
        decimal subtotal = 200m;

        _couponRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Coupon, bool>>>()))
                              .Returns(Task.FromResult<Coupon?>(null));
        _promotionRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Promotion, bool>>>()))
                                 .Returns(Task.FromResult<Promotion?>(null));

        // Act
        var result = await _discountService.ApplyDiscountsAsync(order, subtotal);

        // Assert
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public async Task ApplyDiscountsAsync_WithValidCouponsAndNoPromotion_ReturnsCouponDiscount()
    {
        // Arrange
        var order = new Order
        {
            OrderDate = DateTime.Now,
            OrderItems = new List<OrderItem>
            {
                new OrderItem { ProductId = 1 },
                new OrderItem { ProductId = 2 }
            }
        };
        decimal subtotal = 200m;
        decimal expectedDiscount = 20m;

        var coupon = new Coupon { Code = "MM", DiscountAmount = 10m, ValidFrom = DateTime.Now.AddDays(-1), ValidUntil = DateTime.Now.AddDays(1) };
        _couponRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Coupon, bool>>>()))
                              .ReturnsAsync(coupon);

        _promotionRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Promotion, bool>>>()))
                                 .Returns(Task.FromResult<Promotion?>(null));

        // Act
        var result = await _discountService.ApplyDiscountsAsync(order, subtotal);

        // Assert
        Assert.AreEqual(expectedDiscount, result);
    }

    [TestMethod]
    public async Task ApplyDiscountsAsync_WithNoCouponsAndValidPromotion_ReturnsPromotionDiscount()
    {
        // Arrange
        var order = new Order
        {
            OrderDate = DateTime.Now,
            OrderItems =
            [
                new OrderItem { ProductId = 1 },
                new OrderItem { ProductId = 2 }
            ]
        };
        decimal subtotal = 200m;
        decimal expectedDiscount = 10m;

        _couponRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Coupon, bool>>>()))
                              .Returns(Task.FromResult<Coupon?>(null));

        var promotion = new Promotion { Name = "Active", DiscountPercentage = 5m, ValidFrom = DateTime.Now.AddDays(-1), ValidUntil = DateTime.Now.AddDays(1) };
        _promotionRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Promotion, bool>>>()))
                                 .ReturnsAsync(promotion);

        // Act
        var result = await _discountService.ApplyDiscountsAsync(order, subtotal);

        // Assert
        Assert.AreEqual(expectedDiscount, result);
    }
}
