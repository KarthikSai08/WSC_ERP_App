using WSC.Delivery.Domain.Enums;


namespace WSC.Delivery.Application.Dtos
{
    public class UpdateOrderDeliveryDto
    {
        public int DeliveryId { get; set; }
        public int? AssignedAgentId { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public DeliveryStatus Status { get; set; }
        public string TrackingNumber { get; set; }
    }
}
