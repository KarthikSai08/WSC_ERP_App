using WSC.Gateway.Application.Dtos.AggregatorDtos;
using WSC.Gateway.Application.Interfaces;
using WSC.Shared.Contracts.Common;

namespace WSC.Gateway.Application.Services
{
    public class AggregatorService : IAggregatorService
    {
        public Task<ApiResponse<AppSummaryDto>> GetAppSummaryAsync<T>(string userId, string role, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
