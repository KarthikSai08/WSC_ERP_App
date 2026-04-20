namespace WSC.Delivery.Application.Dtos
{
    public sealed record CreateDeliveryItemDto(int DeliveryId, int ProductId, int Quantity);
}
