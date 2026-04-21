using System;
using System.Collections.Generic;
using System.Text;
using WSC.Shared.Contracts.Dtos.CRMLayer;

namespace WSC.Gateway.Application.Dtos
{
    public record CrmSummaryDto
    {
        public int TotalCustomers { get; set; }
        public int TotalLeads { get; set; }
        public int TotalOpportunities { get; set; }
        public IEnumerable<CustomerResponseDto> RecentCustomers { get; set; }
    }
}
