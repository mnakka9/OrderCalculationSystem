using OCS.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace OCS.Infrastructure.Services.Features.Orders;
[Mapper]
public static partial class OrderMapper
{
    public static partial OrderDto ToDto(this Order order);

    public static partial Order Map(OrderDto orderDto);

    public static partial OrderItem Map(OrderItemDto orderItemDto);

    public static partial OrderItemDto ToDto(OrderItem itemDto);
}
