using WSC.Delivery.Domain.Enums;

namespace WSC.Delivery.Application.Dtos
{
    public sealed record CreateDeliveryTrackingDto(
        int DeliveryId,
        DeliveryStatus Status,
        string Location,
        string Remarks);
}
