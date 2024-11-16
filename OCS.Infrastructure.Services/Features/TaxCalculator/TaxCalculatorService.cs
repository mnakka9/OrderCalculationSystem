using OCS.Domain.Entities;
using OCS.Infrastructure.Services.Factory;

namespace OCS.Infrastructure.Services.Features.TaxCalculator;
public class TaxCalculatorService(ITaxCalculatorFactory taxCalculatorFactory) : ITaxCalculatorService
{
    public decimal CalculateTax(Client client, decimal amount)
    {
        var taxCalculator = taxCalculatorFactory.CreateTaxCalculator(client.State);
        return taxCalculator.CalculateTax(amount);
    }
}