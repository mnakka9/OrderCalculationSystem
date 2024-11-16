namespace OCS.Infrastructure.Services.Factory.Strategies;

public class StandardTaxStrategy(decimal taxRate) : ITaxStrategy
{
    public decimal CalculateTax(decimal amount)
    {
        return amount * taxRate;
    }
}

public class LuxuryItemTaxStrategy(decimal taxRate) : ITaxStrategy
{
    public decimal CalculateTax(decimal amount)
    {
        return amount * taxRate * 2;
    }
}

public class PreDiscountTaxStrategy(decimal taxRate) : ITaxStrategy
{
    public decimal CalculateTax(decimal amount)
    {
        // Assuming the pre-discount amount is passed
        return amount * taxRate;
    }
}

