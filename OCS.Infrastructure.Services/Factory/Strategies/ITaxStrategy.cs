namespace OCS.Infrastructure.Services.Factory.Strategies;

public interface ITaxStrategy
{
    decimal CalculateTax(decimal amount);
}
