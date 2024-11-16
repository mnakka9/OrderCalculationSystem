using Microsoft.EntityFrameworkCore;
using OCS.Domain.Entities;

namespace OCS.Infrastructure.DataAccess;

#nullable disable
public class TaxSystemContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Coupon> Coupons { get; set; }
    public DbSet<Promotion> Promotions { get; set; }
    public DbSet<Client> Clients { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=taxsystem.db");
    }
}
