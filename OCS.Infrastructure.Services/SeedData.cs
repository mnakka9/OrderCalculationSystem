using Microsoft.EntityFrameworkCore;
using OCS.Domain.Entities;
using OCS.Infrastructure.DataAccess;

namespace OCS.Infrastructure.Services;

// seeding required data
public static class SeedData
{
    public static async Task SeedDataAsync(TaxSystemContext context, int start = 1, int end = 10)
    {
        string[] states = ["FL", "NM", "NV", "GA", "NY"];

        if (!context.Clients.Any())
        {
            List<Client> clients = [];

            foreach (string state in states)
            {
                var client = new Client
                {
                    Name = $"Client_{state}",
                    State = state
                };

                clients.Add(client);
            }

            await context.Clients.AddRangeAsync(clients);
            context.SaveChanges();

            Promotion promotion = new()
            {
                Name = "Active promotion",
                DiscountPercentage = 9,
                ValidFrom = DateTime.Now.AddDays(-5),
                ValidUntil = DateTime.Now.AddDays(5)
            };

            await context.Promotions.AddAsync(promotion);
            await context.SaveChangesAsync();


        }

        Random random = new();
        if (!context.Products.Any())
        {
            var products = Enumerable.Range(1, 42).Select(x => new Product
            {
                Name = $"Product_{x}",
                IsLuxuryItem = IsPrime(x),
                Price = IsPrime(x) ? x * random.Next(500, 999) : x * random.Next(99, 200),
            });

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();

            List<Coupon> coupons = [];

            foreach (var number in Enumerable.Range(2, 35))
            {
                if (IsPrime(number))
                {
                    Coupon coupon = new()
                    {
                        Code = "100Discount",
                        DiscountAmount = 100,
                        ValidUntil = DateTime.Now.AddDays(3),
                        ValidFrom = DateTime.Now.AddDays(-3),
                        ProductId = number
                    };
                    coupons.Add(coupon);
                }
                else if (random.Next(2, 35) == number)
                {
                    Coupon coupon = new()
                    {
                        Code = "20Discount",
                        DiscountAmount = 20,
                        ValidUntil = DateTime.Now.AddDays(3),
                        ValidFrom = DateTime.Now.AddDays(-1),
                        ProductId = number
                    };
                    coupons.Add(coupon);
                }
            }

            await context.Coupons.AddRangeAsync(coupons);
            await context.SaveChangesAsync();
        }

        List<OrderItem> orderItems = [];

        foreach (var number in Enumerable.Range(start, end))
        {
            var product = await context.Products.FirstAsync(x => x.Id == number)!;
            var quantity = random.Next(start, end - 5 > start ? end - 5 : start + 2);

            var item = new OrderItem
            {
                Quantity = quantity,
                Price = product.Price * quantity,
                ProductId = product.Id,
                Product = product
            };

            orderItems.Add(item);
        }

        var order = new Order
        {
            ClientId = start,
            OrderDate = DateTime.Now,
            OrderItems = orderItems
        };

        await context.Orders.AddAsync(order);

        await context.SaveChangesAsync();
    }

    static bool IsPrime(int number)
    {

        if (number <= 1) return false;

        return Enumerable.Range(2, (int)Math.Sqrt(number) - 1).All(divisor => number % divisor != 0);

    }
}
