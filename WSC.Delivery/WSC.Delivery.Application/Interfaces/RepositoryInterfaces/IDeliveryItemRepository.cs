using System;
using System.Collections.Generic;
using System.Text;
using WSC.Delivery.Domain.Entities;
using WSC.Shared.Contracts.Dtos.DeliveryLayer;

namespace WSC.Delivery.Application.Interfaces.RepositoryInterfaces
{
    public interface IDeliveryItemRepository
    {
        Task<int> CreateDeliveryItemAsync(DeliveryItem item, CancellationToken ct);
        Task<IEnumerable<DeliveryItemResponseDto>> GetItemsByDeliveryIdAsync(int deliveryId, CancellationToken ct);
        Task<DeliveryItemResponseDto> GetItemByIdAsync(int itemId, CancellationToken ct);
        Task<DeliveryItem> GetItemEntityByIdAsync(int itemId, CancellationToken ct);
        Task<bool> UpdateDeliveryItemAsync(DeliveryItem item, CancellationToken ct);
        Task<bool> DeleteDeliveryItemAsync(int itemId, CancellationToken ct);
    }
}
