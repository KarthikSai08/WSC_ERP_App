using System;
using System.Collections.Generic;
using System.Text;

namespace WSC.Gateway.Application.Dtos
{
    public record DeliverySummaryDto
    {
        public int TotalDeliveries { get; set; }
        public int ActiveAgents { get; set; }
        public IEnumerable<DeliveryResponseDto> RecentDeliveries { get; set; }
    }
}
