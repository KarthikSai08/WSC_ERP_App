using System;
using System.Collections.Generic;
using System.Text;

namespace WSC.Delivery.Domain.Entities
{
    public class Delivery
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
