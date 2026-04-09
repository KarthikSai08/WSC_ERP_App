using WSC.Delivery.Domain.Enums;

namespace WSC.Delivery.Domain.Entities
{
    public class OrderDelivery
    {
        public int DeliveryId { get; set; }
        public int OrderId { get; set; }          // From Store module
        public int CustomerId { get; set; }       // From CRM module
        public string TrackingNumber { get; set; }

        public DeliveryStatus Status { get; set; }

        public int? AssignedAgentId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public string DeliveryAddress { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
