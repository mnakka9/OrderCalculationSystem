using Microsoft.Extensions.DependencyInjection;
using OCS.Infrastructure.DataAccess;
using OCS.Infrastructure.DataAccess.Repositories;
using OCS.Infrastructure.DataAccess.Repositories.Clients;
using OCS.Infrastructure.DataAccess.Repositories.Coupons;
using OCS.Infrastructure.DataAccess.Repositories.Orders;
using OCS.Infrastructure.DataAccess.Repositories.Products;
using OCS.Infrastructure.DataAccess.Repositories.Promotions;
using OCS.Infrastructure.Services.Factory;
using OCS.Infrastructure.Services.Features.Discount;
using OCS.Infrastructure.Services.Features.OrderCalculator;
using OCS.Infrastructure.Services.Features.Orders;
using OCS.Infrastructure.Services.Features.TaxCalculator;

namespace OCS.Infrastructure.Services;
public static class ServiceExtensions
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<ICouponRepository, CouponRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IPromotionRepository, PromotionRepository>();

        services.AddDbContext<TaxSystemContext>();
    }

    public static void AddRequiredServices(this IServiceCollection services)
    {
        services.AddScoped<ITaxCalculatorFactory, TaxCalculatorFactory>();
        services.AddScoped<IOrderCalculatorService, OrderCalculatorService>();
        services.AddScoped<ITaxCalculatorService, TaxCalculatorService>();
        services.AddScoped<IDiscountService, DiscountService>();
        services.AddScoped<IOrderService, OrderService>();
    }
}
