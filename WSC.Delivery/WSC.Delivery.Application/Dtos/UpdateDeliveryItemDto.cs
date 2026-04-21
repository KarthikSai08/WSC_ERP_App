namespace WSC.Delivery.Application.Dtos
{
    public sealed record UpdateDeliveryItemDto(int DeliveryItemId,
                                                int Quantity,
                                                string Description,
                                                bool IsDelivered);
}
