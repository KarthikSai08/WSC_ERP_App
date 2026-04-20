using WSC.Delivery.Domain.Enums;


namespace WSC.Delivery.Application.Dtos
{
    public sealed record UpdateOrderDeliveryDto(
         int DeliveryId,
         int? AssignedAgentId,
         DateTime? ScheduledDate,
         DeliveryStatus Status,
         string TrackingNumber);
}
