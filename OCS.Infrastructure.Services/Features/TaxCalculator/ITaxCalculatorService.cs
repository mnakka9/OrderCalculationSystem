using OCS.Domain.Entities;

namespace OCS.Infrastructure.Services.Features.TaxCalculator;

public interface ITaxCalculatorService
{
    decimal CalculateTax(Client client, decimal amount);
}
