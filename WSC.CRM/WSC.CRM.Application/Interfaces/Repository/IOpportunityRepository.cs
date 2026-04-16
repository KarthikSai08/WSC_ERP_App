using WSC.CRM.Domain.Entities;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.CRMLayer;

namespace WSC.CRM.Application.Interfaces.Repository
{
    public interface IOpportunityRepository
    {
        Task<IEnumerable<OpportunityResponseDto>> GetAllOpportunitiesAsync(CancellationToken ct);
        Task<OpportunityResponseDto?> GetOpportunityByIdAsync(int id, CancellationToken ct);
        Task<Opportunity?> GetOpportunityEntityByIdAsync(int id, CancellationToken ct);
        Task<int> CreateOpportunityAsync(Opportunity opp, CancellationToken ct);
        Task<bool> UpdateOpportunityAsync(Opportunity opp, CancellationToken ct);
        Task<bool> DeleteOpportunityAsync(int id, CancellationToken ct);
        Task<IEnumerable<OpportunityResponseDto>> GetAllAOpportunitiesByFilterAsync(CancellationToken ct);
        Task<IEnumerable<OpportunityResponseDto>> GetOpportunitiesByCustomerIdAsync(int cxId, CancellationToken ct);
        Task<bool> UpdateClosedAtAsync(int oppId, CancellationToken ct);
        Task<(IEnumerable<OpportunityResponseDto> Data, int TotalCount)> GetPagedOpportunitiesAsync(PaginationRequest request, CancellationToken ct);
    }
}
