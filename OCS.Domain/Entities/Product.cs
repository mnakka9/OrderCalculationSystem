using System.ComponentModel.DataAnnotations;

namespace OCS.Domain.Entities;

public class Product
{
    [Key]
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public bool IsLuxuryItem { get; set; }
}
