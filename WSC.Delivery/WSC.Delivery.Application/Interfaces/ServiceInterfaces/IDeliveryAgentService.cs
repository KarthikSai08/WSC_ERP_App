using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.DeliveryLayer;
using WSC.Delivery.Application.Dtos;

namespace WSC.Delivery.Application.Interfaces.ServiceInterfaces
{
    public interface IDeliveryAgentService
    {
        Task<ApiResponse<int>> CreateDeliveryAgentAsync(CreateDeliveryAgentDto dto, CancellationToken ct);
        Task<ApiResponse<IEnumerable<DeliveryAgentResponseDto>>> GetAllAgentsAsync(CancellationToken ct);
        Task<ApiResponse<DeliveryAgentResponseDto>> GetAgentByIdAsync(int agentId, CancellationToken ct);
        Task<ApiResponse<bool>> UpdateDeliveryAgentAsync(UpdateDeliveryAgentDto dto, CancellationToken ct);
        Task<ApiResponse<bool>> DeleteAgentAsync(int agentId, CancellationToken ct);
        Task<ApiResponse<IEnumerable<DeliveryAgentResponseDto>>> GetAvailableAgentsAsync(CancellationToken ct);
    }
}
