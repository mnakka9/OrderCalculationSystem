using System.ComponentModel.DataAnnotations;

namespace OCS.Domain.Entities;

public class Client
{
    [Key]
    public int Id { get; set; }

    [MaxLength(200)]
    public required string Name { get; set; }

    [MaxLength(10)]
    public required string State { get; set; }
}