using WSC.Store.Domain.Enums;

namespace WSC.Store.Application.Dtos
{
    public sealed record UpdateOrderDto(int OrderId, int CustomerId, decimal TotalAmount, OrderStatus Status);
}
