namespace WSC.Shared.Contracts.Dtos.DeliveryLayer
{
    public class CreateOrderDeliveryDto
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string TrackingNumber { get; set; }
        public int? AssignedAgentId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string DeliveryAddress { get; set; }
    }
}
