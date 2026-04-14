using System;
using System.Collections.Generic;
using System.Text;

namespace WSC.Dashboard.Application.Dtos
{
    public class AgentDisplayDto
    {
        public int DeliveryAgentId { get; set; }
        public string AgentName { get; set; }
        public string AgentPhone { get; set; }
        public string VehicleNumber { get; set; }
    }
}
