namespace OCS.Infrastructure.Services.Features.Orders;
public class OrderDto
{
    public int Id { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal DiscountAmount { get; set; }

    public decimal PreTaxAmount { get; set; }
    public int? ClientId { get; set; }

    public List<OrderItemDto> OrderItems { get; set; } = [];

    public DateTime OrderDate { get; set; } = DateTime.Now;
}

public class OrderItemDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
