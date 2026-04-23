using WSC.Shared.Contracts.Dtos.CRMLayer;

namespace WSC.Gateway.Application.Interfaces.Clients
{
    public interface ICrmAggregatorClient
    {
        Task<IEnumerable<CustomerResponseDto>?> GetAllCustomerAsync(CancellationToken ct);
        Task<IEnumerable<LeadResponseDto>?> GetAllLeadAsync(CancellationToken ct);
        Task<IEnumerable<OpportunityResponseDto>?> GetAllOpportunityAsync(CancellationToken ct);
    }
}
