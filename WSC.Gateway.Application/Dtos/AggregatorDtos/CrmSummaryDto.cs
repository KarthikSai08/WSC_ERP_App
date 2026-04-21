using WSC.Shared.Contracts.Dtos.CRMLayer;

namespace WSC.Gateway.Application.Dtos.AggregatorDtos
{
    public record CrmSummaryDto
    {
        public int TotalCustomers { get; set; }
        public int TotalLeads { get; set; }
        public int TotalOpportunities { get; set; }
        public IEnumerable<CustomerResponseDto> RecentCustomers { get; set; }
    }
}
