using WSC.Delivery.Domain.Enums;
using WSC.Shared.Contracts.Enums;

namespace WSC.Dashboard.Application.Dtos
{
    public sealed record CustomerOrderDetailsDto
    {
        public int CxId { get; set; }
        public string CxName { get; set; } = null!;
        public string CxEmail { get; set; } = null!;
        public string CxPhone { get; set; }
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus OrderStatus { get; set; }
        //public List<OrderItemsDto> OrderItems { get; set; }
        public int DeliveryId { get; set; }
        public string TrackingNumber { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
        public int? AssignedAgentId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string AgentName { get; set; }
        public string AgentPhone { get; set; }
        public string VehicleNumber { get; set; }

    }
}
