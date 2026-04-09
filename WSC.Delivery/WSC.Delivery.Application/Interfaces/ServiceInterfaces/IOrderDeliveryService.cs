using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.DeliveryLayer;
using WSC.Delivery.Application.Dtos;

namespace WSC.Delivery.Application.Interfaces.ServiceInterfaces
{
    public interface IOrderDeliveryService
    {
        Task<ApiResponse<int>> CreateOrderDeliveryAsync(CreateOrderDeliveryDto dto, CancellationToken ct);
        Task<ApiResponse<IEnumerable<OrderDeliveryResponseDto>>> GetAllDeliveriesAsync(CancellationToken ct);
        Task<ApiResponse<OrderDeliveryResponseDto>> GetDeliveryByIdAsync(int deliveryId, CancellationToken ct);
        Task<ApiResponse<bool>> UpdateOrderDeliveryAsync(UpdateOrderDeliveryDto dto, CancellationToken ct);
        Task<ApiResponse<bool>> DeleteDeliveryAsync(int deliveryId, CancellationToken ct);
        Task<ApiResponse<IEnumerable<OrderDeliveryResponseDto>>> GetDeliveriesByStatusAsync(string status, CancellationToken ct);
        Task<ApiResponse<IEnumerable<OrderDeliveryResponseDto>>> GetDeliveriesByAgentIdAsync(int agentId, CancellationToken ct);
    }
}
