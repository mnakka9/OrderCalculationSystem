using System.ComponentModel.DataAnnotations;

namespace OCS.Domain.Entities;

public class Coupon
{
    [Key]
    public int Id { get; set; }
    public required string Code { get; set; }
    public decimal DiscountAmount { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime ValidUntil { get; set; }

    public int? ProductId { get; set; }
}
