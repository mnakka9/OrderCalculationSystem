using Moq;
using OCS.Domain.Entities;
using OCS.Infrastructure.DataAccess.Repositories.Orders;
using OCS.Infrastructure.DataAccess.Repositories.Products;
using OCS.Infrastructure.Services.Features.OrderCalculator;
using OCS.Infrastructure.Services.Features.Orders;

namespace OCS.Infra.Tests.Orders;


[TestClass]
public class OrderServiceTests
{
    private Mock<IOrderRepository> _orderRepositoryMock;
    private Mock<IProductRepository> _productRepositoryMock;
    private Mock<IOrderCalculatorService> _calculatorServiceMock;
    private OrderService _orderService;

    [TestInitialize]
    public void Setup()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _calculatorServiceMock = new Mock<IOrderCalculatorService>();

        _orderService = new OrderService(
            _orderRepositoryMock.Object,
            _productRepositoryMock.Object,
            _calculatorServiceMock.Object
        );
    }

    [TestMethod]
    public async Task SaveOrderAsync_ValidOrder_ReturnsOrderDto()
    {
        // Arrange
        var orderDto = new OrderDto { Id = 1 };
        var order = OrderMapper.Map(orderDto);
        var calculatedOrder = new OrderDto { Id = order.Id, /* Other properties */ };

        _orderRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Order>()))
                            .Returns(Task.CompletedTask);
        _calculatorServiceMock.Setup(service => service.CalculateOrderTotalsAsync(order.Id))
                              .ReturnsAsync(calculatedOrder);

        // Act
        var result = await _orderService.SaveOrderAsync(orderDto);

        // Assert
        Assert.IsNotNull(result);
        _orderRepositoryMock.Verify(repo => repo.CreateAsync(It.Is<Order>(o => o.Id == order.Id)), Times.Once);
        _calculatorServiceMock.Verify(service => service.CalculateOrderTotalsAsync(order.Id), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(KeyNotFoundException))]
    public async Task SaveOrderAsync_InvalidProduct_ThrowsKeyNotFoundException()
    {
        // Arrange
        var orderDto = new OrderDto
        {
            Id = 1,
            ClientId = 1,
            OrderItems = [
            new OrderItemDto { Id = 1, ProductId = 1 },
            ]
        };

        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                              .Returns(Task.FromResult<Product?>(null));

        // Act
        await _orderService.SaveOrderAsync(orderDto);

        // Assert
        // Exception is expected
    }

    [TestMethod]
    public async Task SaveOrderAsync_OrderCalculationFails_ThrowsException()
    {
        // Arrange
        var orderDto = new OrderDto { /* Initialize properties */ };
        var order = OrderMapper.Map(orderDto);

        _orderRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Order>()))
                            .Returns(Task.CompletedTask);
        _calculatorServiceMock.Setup(service => service.CalculateOrderTotalsAsync(order.Id))
                              .ThrowsAsync(new Exception("Calculation error"));

        // Act & Assert
        await Assert.ThrowsExceptionAsync<Exception>(() => _orderService.SaveOrderAsync(orderDto));
    }
}
