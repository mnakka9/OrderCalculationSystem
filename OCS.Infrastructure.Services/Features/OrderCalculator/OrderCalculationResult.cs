namespace OCS.Infrastructure.Services.Features.OrderCalculator;

public class OrderCalculationResult
{
    public decimal Subtotal { get; internal set; }
    public decimal Discount { get; internal set; }
    public decimal Tax { get; internal set; }
    public decimal Total { get; internal set; }

    public decimal TaxableAmount { get; internal set; }
}