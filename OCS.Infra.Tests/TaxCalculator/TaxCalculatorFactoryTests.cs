using OCS.Infrastructure.Services.Factory;
using OCS.Infrastructure.Services.Factory.Strategies;

namespace OCS.Infra.Tests.TaxCalculator;

[TestClass]
public class TaxCalculatorFactoryTests
{
    private TaxCalculatorFactory? _factory;

    [TestInitialize]
    public void Setup()
    {
        _factory = new TaxCalculatorFactory();
    }

    [TestMethod]
    public void CreateTaxCalculator_GA_ReturnsStandardTaxStrategy()
    {
        // Act
        var strategy = _factory!.CreateTaxCalculator("GA");

        // Assert
        Assert.IsInstanceOfType<StandardTaxStrategy>(strategy);
    }

    [TestMethod]
    public void CreateTaxCalculator_FL_ReturnsPreDiscountTaxStrategy()
    {
        // Act
        var strategy = _factory!.CreateTaxCalculator("FL");

        // Assert
        Assert.IsInstanceOfType<PreDiscountTaxStrategy>(strategy); ;
    }

    [TestMethod]
    public void CreateTaxCalculator_NM_ReturnsPreDiscountTaxStrategy()
    {
        // Act
        var strategy = _factory!.CreateTaxCalculator("NM");

        // Assert
        Assert.IsInstanceOfType<PreDiscountTaxStrategy>(strategy);
    }

    [TestMethod]
    public void CreateTaxCalculator_NV_ReturnsPreDiscountTaxStrategy()
    {
        // Act
        var strategy = _factory!.CreateTaxCalculator("NV");

        // Assert
        Assert.IsInstanceOfType<PreDiscountTaxStrategy>(strategy);
    }

    [TestMethod]
    public void CreateTaxCalculator_NY_ReturnsStandardTaxStrategy()
    {
        // Act
        var strategy = _factory!.CreateTaxCalculator("NY");

        // Assert
        Assert.IsInstanceOfType<StandardTaxStrategy>(strategy);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CreateTaxCalculator_InvalidState_ThrowsArgumentException()
    {
        // Act
        _factory!.CreateTaxCalculator("INVALID_STATE");

        // Assert
        // Exception is expected
    }
}

