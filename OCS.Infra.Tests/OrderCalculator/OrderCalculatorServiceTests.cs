using Moq;
using OCS.Domain.Entities;
using OCS.Infrastructure.DataAccess.Repositories.Orders;
using OCS.Infrastructure.Services.Features.Discount;
using OCS.Infrastructure.Services.Features.OrderCalculator;
using OCS.Infrastructure.Services.Features.TaxCalculator;

namespace OCS.Infra.Tests.OrderCalculator;

[TestClass]
public class OrderCalculatorServiceTests
{
    private Mock<IDiscountService> _discountServiceMock;
    private Mock<ITaxCalculatorService> _taxCalculatorServiceMock;
    private Mock<IOrderRepository> _orderRepositoryMock;
    private OrderCalculatorService _orderCalculatorService;

    [TestInitialize]
    public void Setup()
    {
        _discountServiceMock = new Mock<IDiscountService>();
        _taxCalculatorServiceMock = new Mock<ITaxCalculatorService>();
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _orderCalculatorService = new OrderCalculatorService(
            _discountServiceMock.Object,
            _taxCalculatorServiceMock.Object,
            _orderRepositoryMock.Object
        );
    }

    [TestMethod]
    public async Task CalculateOrderTotalsAsync_ValidOrderId_ReturnsUpdatedOrder()
    {
        // Arrange
        int orderId = 1;
        var order = new Order
        {
            Id = orderId,
            Client = new Client { Name = "GA_Client", State = "GA" },
            OrderDate = DateTime.Now,
            OrderItems =
            [
                new OrderItem { ProductId = 1, Quantity = 2, Product = new Product { Name ="One", Price = 50m } },
                new OrderItem { ProductId = 2, Quantity = 1, Product = new Product { Name="Two", Price = 100m } }
            ]
        };

        decimal subtotal = 200m;
        decimal discount = 20m;
        decimal tax = 14.4m;
        decimal total = 194.4m;

        _orderRepositoryMock.Setup(repo => repo.GetOrderByIdAsync(orderId))
                            .ReturnsAsync(order);

        _discountServiceMock.Setup(service => service.ApplyDiscountsAsync(order, subtotal))
                            .ReturnsAsync(discount);

        _taxCalculatorServiceMock.Setup(service => service.CalculateTax(order.Client, subtotal - discount))
                                 .Returns(tax);

        _orderRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Order>()))
                            .Returns(Task.CompletedTask);

        // Act
        var result = await _orderCalculatorService.CalculateOrderTotalsAsync(orderId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(total, result.TotalAmount);
        Assert.AreEqual(tax, result.TaxAmount);
        Assert.AreEqual(discount, result.DiscountAmount);
        _orderRepositoryMock.Verify(repo => repo.GetOrderByIdAsync(orderId), Times.Once);
        _orderRepositoryMock.Verify(repo => repo.UpdateAsync(order), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(KeyNotFoundException))]
    public async Task CalculateOrderTotalsAsync_InvalidOrderId_ThrowsKeyNotFoundException()
    {
        // Arrange
        int orderId = 1;

        _orderRepositoryMock.Setup(repo => repo.GetOrderByIdAsync(orderId))
                            .Returns(Task.FromResult<Order?>(null));

        // Act
        await _orderCalculatorService.CalculateOrderTotalsAsync(orderId);

        // Assert
        // Exception is expected
    }

    [TestMethod]
    public async Task CalculateOrderAsync_ValidOrder_ReturnsOrderCalculationResult()
    {
        // Arrange
        var order = new Order
        {
            Client = new Client { Name = "Test", State = "GA" },
            OrderDate = DateTime.Now,
            OrderItems = new List<OrderItem>
            {
                new OrderItem { ProductId = 1, Quantity = 2, Product = new Product { Name="One", Price = 50m } },
                new OrderItem { ProductId = 2, Quantity = 1, Product = new Product { Name="Two", Price = 100m } }
            }
        };

        decimal subtotal = 200m;
        decimal discount = 20m;
        decimal tax = 14.4m;
        decimal total = 194.4m;

        _discountServiceMock.Setup(service => service.ApplyDiscountsAsync(order, subtotal))
                            .ReturnsAsync(discount);

        _taxCalculatorServiceMock.Setup(service => service.CalculateTax(order.Client, subtotal - discount))
                                 .Returns(tax);

        // Act
        var result = await InvokeCalculateOrderAsync(_orderCalculatorService, order);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(subtotal, result.Subtotal);
        Assert.AreEqual(discount, result.Discount);
        Assert.AreEqual(tax, result.Tax);
        Assert.AreEqual(total, result.Total);
        Assert.AreEqual(subtotal - discount, result.TaxableAmount); // Discount applied before tax

    }

    [TestMethod]
    public async Task CalculateOrderAsync_ValidOrderWithPreTaxDiscountState_ReturnsOrderCalculationResult()
    {
        // Arrange
        var order = new Order
        {
            Client = new Client { Name = "Test", State = "FL" },
            OrderDate = DateTime.Now,
            OrderItems =
            [
                new OrderItem { ProductId = 1, Quantity = 2, Product = new Product { Name="One", Price = 50m } },
                new OrderItem { ProductId = 2, Quantity = 1, Product = new Product { Name="Two", Price = 100m } }
            ]
        };

        decimal subtotal = 200m;
        decimal discount = 20m;
        decimal tax = 30m;
        decimal total = 210m;

        _discountServiceMock.Setup(service => service.ApplyDiscountsAsync(order, subtotal))
                            .ReturnsAsync(discount);

        // we are not deleting discount from subtotal here because
        // tax is applied before discount for FL
        _taxCalculatorServiceMock.Setup(service => service.CalculateTax(order.Client, subtotal))
                                 .Returns(tax);

        // Act
        var result = await InvokeCalculateOrderAsync(_orderCalculatorService, order);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(subtotal, result.Subtotal);
        Assert.AreEqual(discount, result.Discount);
        Assert.AreEqual(tax, result.Tax);
        Assert.AreEqual(total, result.Total);
        Assert.AreEqual(subtotal, result.TaxableAmount); // discount not applied before tax
    }

    private static async Task<OrderCalculationResult> InvokeCalculateOrderAsync(OrderCalculatorService service, Order order)
    {
        var method = typeof(OrderCalculatorService)
                     .GetMethod("CalculateOrderAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return await (Task<OrderCalculationResult>)method!.Invoke(service, [order])!;
    }
}
