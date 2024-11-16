using OCS.Infrastructure.Services.Factory.Strategies;

namespace OCS.Infrastructure.Services.Factory;

public interface ITaxCalculatorFactory
{
    ITaxStrategy CreateTaxCalculator(string state);
}
