using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.DeliveryLayer;
using WSC.Delivery.Application.Dtos;

namespace WSC.Delivery.Application.Interfaces.ServiceInterfaces
{
    public interface IDeliveryTrackingService
    {
        Task<ApiResponse<int>> CreateTrackingRecordAsync(CreateDeliveryTrackingDto dto, CancellationToken ct);
        Task<ApiResponse<IEnumerable<DeliveryTrackingResponseDto>>> GetTrackingByDeliveryIdAsync(int deliveryId, CancellationToken ct);
        Task<ApiResponse<DeliveryTrackingResponseDto>> GetTrackingByIdAsync(int trackingId, CancellationToken ct);
        Task<ApiResponse<IEnumerable<DeliveryTrackingResponseDto>>> GetLatestTrackingAsync(int deliveryId, int count, CancellationToken ct);
    }
}
