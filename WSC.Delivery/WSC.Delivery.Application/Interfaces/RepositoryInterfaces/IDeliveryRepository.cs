using System;
using System.Collections.Generic;
using System.Text;
using WSC.Delivery.Domain.Entities;
using WSC.Shared.Contracts.Dtos.DeliveryLayer;
namespace WSC.Delivery.Application.Interfaces.RepositoryInterfaces
{
    public interface IDeliveryRepository
    {
        Task<bool> CreateDeliveryAsync(OrderDelivery delivery, CancellationToken ct);
        Task<IEnumerable<OrderDeliveryResponseDto>> GetAllDeliveriesAsync(CancellationToken ct);
        Task<OrderDeliveryResponseDto> GetByDeliveryIdAsync(int deliveryId, CancellationToken ct);
        Task<OrderDelivery> GetDeliveryEntityByIdAsync(int deliveryId, CancellationToken ct);
        Task<bool> DeleteDeliveryByIdAsync(int deliveryId, CancellationToken ct);
        Task<OrderDelivery> GetByDeliveryStatusAsync(string sts, CancellationToken ct);
        Task<bool> UpdateDeliveryDetailsAsync(OrderDelivery delivery, CancellationToken ct);
        Task<bool> UpdateDeliveryStatusAsync(string sts, CancellationToken ct);



    }
}
