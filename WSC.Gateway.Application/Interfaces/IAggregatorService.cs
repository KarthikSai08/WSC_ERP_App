using WSC.Gateway.Application.Dtos.AggregatorDtos;
using WSC.Shared.Contracts.Common;

namespace WSC.Gateway.Application.Interfaces
{
    public interface IAggregatorService
    {
        Task<ApiResponse<AppSummaryDto>> GetAppSummaryAsync<T>(string userId, string role, CancellationToken ct);
    }
}
