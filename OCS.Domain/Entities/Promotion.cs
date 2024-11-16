using System.ComponentModel.DataAnnotations;

namespace OCS.Domain.Entities;

public class Promotion
{
    [Key]
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal DiscountPercentage { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime ValidUntil { get; set; }
}
