using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OCS.Domain.Entities;
public class Order
{
    [Key]
    public int Id { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public int? ClientId { get; set; }

    [ForeignKey(nameof(ClientId))]
    public Client? Client { get; set; }
    public List<OrderItem> OrderItems { get; set; } = [];

    public DateTime OrderDate { get; set; } = DateTime.Now;
}
