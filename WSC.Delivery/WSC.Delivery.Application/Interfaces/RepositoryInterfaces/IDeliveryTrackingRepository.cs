using System;
using System.Collections.Generic;
using System.Text;
using WSC.Delivery.Domain.Entities;
using WSC.Shared.Contracts.Dtos.DeliveryLayer;

namespace WSC.Delivery.Application.Interfaces.RepositoryInterfaces
{
    public interface IDeliveryTrackingRepository
    {
        Task<int> CreateTrackingRecordAsync(DeliveryTracking tracking, CancellationToken ct);
        Task<IEnumerable<DeliveryTrackingResponseDto>> GetTrackingByDeliveryIdAsync(int deliveryId, CancellationToken ct);
        Task<DeliveryTrackingResponseDto> GetTrackingByIdAsync(int trackingId, CancellationToken ct);
        Task<DeliveryTracking> GetTrackingEntityByIdAsync(int trackingId, CancellationToken ct);
        Task<IEnumerable<DeliveryTracking>> GetLatestTrackingByDeliveryIdAsync(int deliveryId, int count, CancellationToken ct);
        Task<bool> DeleteTrackingRecordAsync(int trackingId, CancellationToken ct);
    }
}
