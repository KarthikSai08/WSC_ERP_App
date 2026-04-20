namespace WSC.Delivery.Application.Dtos
{
    public sealed class UpdateDeliveryItemDto(int DeliveryItemId, int Quantity, string Description, bool IsDelivered);
}
