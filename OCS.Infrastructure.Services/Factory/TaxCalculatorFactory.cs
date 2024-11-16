using OCS.Infrastructure.Services.Factory.Strategies;

namespace OCS.Infrastructure.Services.Factory;

public class TaxCalculatorFactory : ITaxCalculatorFactory
{
    public ITaxStrategy CreateTaxCalculator(string state)
    {
        // Tax rates are hard coded, can be maintained in database
        return state.ToUpper() switch
        {
            "GA" => new StandardTaxStrategy(0.04m),
            "FL" or "NM" or "NV" => new PreDiscountTaxStrategy(0.05m),
            "NY" => new StandardTaxStrategy(0.045m),
            _ => throw new ArgumentException($"No tax calculator defined for state: {state}"),
        };
    }
}