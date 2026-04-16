using WSC.CRM.Application.Dtos;
using WSC.CRM.Domain.Entities;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.CRMLayer;

namespace WSC.CRM.Application.Interfaces.Services
{
    public interface IOpportunityService
    {
        Task<ApiResponse<IEnumerable<OpportunityResponseDto>>> GetAllOpportunitiesAsync(CancellationToken ct);
        Task<ApiResponse<OpportunityResponseDto?>> GetOpportunityByIdAsync(int id, CancellationToken ct);
        Task<ApiResponse<Opportunity?>> GetOpportunityEntityByIdAsync(int id, CancellationToken ct);
        Task<ApiResponse<int>> CreateOpportunityAsync(CreateOpportunityDto dto, CancellationToken ct);
        Task<ApiResponse<bool>> UpdateOpportunityAsync(UpdateOpportunityDto dto, CancellationToken ct);
        Task<ApiResponse<bool>> DeleteOpportunityAsync(int id, CancellationToken ct);
        Task<ApiResponse<IEnumerable<OpportunityResponseDto>>> GetAllAOpportunitiesByFilterAsync(CancellationToken ct);
        Task<ApiResponse<IEnumerable<OpportunityResponseDto>>> GetOpportunitiesByCustomerIdAsync(int cxId, CancellationToken ct);
        Task<ApiResponse<bool>> UpdateClosedAtAsync(int oppId, CancellationToken ct);
        Task<ApiResponse<PagedResponse<OpportunityResponseDto>>> GetPagedOpportunitiesAsync(PaginationRequest request, CancellationToken ct);
    }
}
