namespace WSC.Shared.Contracts.Dtos.DeliveryLayer
{
    public class UpdateOrderDeliveryDto
    {
        public int DeliveryId { get; set; }
        public string? TrackingNumber { get; set; }
        public int? AssignedAgentId { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public string? DeliveryAddress { get; set; }
    }
}
