using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.DeliveryLayer;
using WSC.Delivery.Application.Dtos;

namespace WSC.Delivery.Application.Interfaces.ServiceInterfaces
{
    public interface IDeliveryItemService
    {
        Task<ApiResponse<int>> CreateDeliveryItemAsync(CreateDeliveryItemDto dto, CancellationToken ct);
        Task<ApiResponse<IEnumerable<DeliveryItemResponseDto>>> GetDeliveryItemsByDeliveryIdAsync(int deliveryId, CancellationToken ct);
        Task<ApiResponse<DeliveryItemResponseDto>> GetDeliveryItemByIdAsync(int itemId, CancellationToken ct);
        Task<ApiResponse<bool>> UpdateDeliveryItemAsync(UpdateDeliveryItemDto dto, CancellationToken ct);
        Task<ApiResponse<bool>> DeleteDeliveryItemAsync(int itemId, CancellationToken ct);
    }
}
