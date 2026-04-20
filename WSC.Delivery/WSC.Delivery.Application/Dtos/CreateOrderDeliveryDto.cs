namespace WSC.Delivery.Application.Dtos
{
    public sealed record CreateOrderDeliveryDto(
        int OrderId,
        int CustomerId,
        int? AssignedAgentId,
        string TrackingNumber,
        DateTime ScheduledDate,
        string DeliveryAddress);
}
