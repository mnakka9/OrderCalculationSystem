using Moq;
using OCS.Domain.Entities;
using OCS.Infrastructure.Services.Factory;
using OCS.Infrastructure.Services.Factory.Strategies;
using OCS.Infrastructure.Services.Features.TaxCalculator;

namespace OCS.Infra.Tests.TaxCalculator;

[TestClass]
public class TaxCalculatorServiceTests
{
    private Mock<ITaxCalculatorFactory>? _taxCalculatorFactoryMock;
    private TaxCalculatorService? _taxCalculatorService;

    [TestInitialize]
    public void Setup()
    {
        _taxCalculatorFactoryMock = new Mock<ITaxCalculatorFactory>();
        _taxCalculatorService = new TaxCalculatorService(_taxCalculatorFactoryMock.Object);
    }

    [TestMethod]
    public void CalculateTax_GivenValidState_ReturnsCorrectTax()
    {
        // Arrange
        var client = new Client { Name = "GA_Client", State = "GA" };
        decimal amount = 100m;
        decimal expectedTax = 4m;

        var taxStrategyMock = new Mock<ITaxStrategy>();
        taxStrategyMock.Setup(ts => ts.CalculateTax(amount)).Returns(expectedTax);

        _taxCalculatorFactoryMock!.Setup(factory => factory.CreateTaxCalculator(client.State))
                                 .Returns(taxStrategyMock.Object);

        // Act
        var result = _taxCalculatorService!.CalculateTax(client, amount);

        // Assert
        Assert.AreEqual(expectedTax, result);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CalculateTax_GivenInvalidState_ThrowsArgumentException()
    {
        // Arrange
        var client = new Client { Name = "Test", State = "INVALID_STATE" };
        decimal amount = 100m;

        _taxCalculatorFactoryMock!.Setup(factory => factory.CreateTaxCalculator(client.State))
                                 .Throws(new ArgumentException("No tax calculator defined for state: INVALID_STATE"));

        // Act
        _taxCalculatorService!.CalculateTax(client, amount);

        // Assert
        // Exception is expected
    }
}
