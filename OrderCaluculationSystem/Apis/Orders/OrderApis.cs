using Microsoft.AspNetCore.Mvc;
using OCS.Infrastructure.Services.Features.OrderCalculator;
using OCS.Infrastructure.Services.Features.Orders;

namespace OrderCalculationSystem.Apis.Orders;

public static class OrderApis
{
    public static void MapOrderApis(this WebApplication app)
    {
        app.MapPost("/orders/{id}/calculate", async (int id, [FromServices] IOrderCalculatorService service, ILogger<WebApplication> logger) =>
        {
            if (id == 0)
            {
                return Results.BadRequest("Order id cannot be zero");
            }

            try
            {
                var result = await service.CalculateOrderTotalsAsync(id);

                return Results.Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogError(ex, "Order not found");

                return Results.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while calculating the tax");

                return Results.StatusCode(500);
            }
        });

        app.MapPost("/orders", async ([FromBody] OrderDto orderDto, [FromServices] IOrderService service, [FromServices] ILogger<WebApplication> logger) =>
        {
            if (orderDto == null || orderDto.OrderItems is null or { Count: 0 })
            {
                return Results.BadRequest("Mandatory fields are missing - OrderItems");
            }

            try
            {
                var result = await service.SaveOrderAsync(orderDto);

                return Results.Created($"/orders/{orderDto.Id}/calculate", orderDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while creating order");

                return Results.BadRequest("Error while creating order");
            }
        });
    }
}
