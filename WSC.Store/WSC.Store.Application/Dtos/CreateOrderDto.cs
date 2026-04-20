using WSC.Store.Domain.Enums;

namespace WSC.Store.Application.Dtos
{
    public sealed record CreateOrderDto(int CustomerId, decimal TotalAmount, OrderStatus Status);
}
