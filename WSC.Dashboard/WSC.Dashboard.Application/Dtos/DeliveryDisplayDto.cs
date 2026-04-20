using WSC.Delivery.Domain.Enums;

namespace WSC.Dashboard.Application.Dtos
{
    public sealed record DeliveryDisplayDto
    {
        public int DeliveryId { get; set; }
        public string TrackingNumber { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
        public DateTime ScheduledDate { get; set; }

        public AgentDisplayDto Agent { get; set; }
    }
}
