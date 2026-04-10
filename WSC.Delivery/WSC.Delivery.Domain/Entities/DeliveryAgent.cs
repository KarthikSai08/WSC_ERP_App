using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WSC.Delivery.Domain.Entities
{
    public class DeliveryAgent
    {
        public int DeliveryAgentId { get; set; }

        public string AgentName { get; set; }
        public string AgentPhone { get; set; }
        public string VehicleNumber { get; set; }

        public bool IsAvailable { get; set; }
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }
    }
}
