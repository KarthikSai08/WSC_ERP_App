using System;
using System.Collections.Generic;
using System.Text;
using WSC.Shared.Contracts.Enums;

namespace WSC.Shared.Contracts.Dtos.DeliveryLayer
{
    public class OrderDeliveryResponseDto
    {
        public int DeliveryId { get; set; }
        public int OrderId { get; set; }          // From Store module
        public int CustomerId { get; set; }       // From CRM module
        public string TrackingNumber { get; set; }
        public DeliveryStatus Status { get; set; }

        public int? AssignedAgentId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
