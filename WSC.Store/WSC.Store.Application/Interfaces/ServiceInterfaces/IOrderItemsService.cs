using System;
using System.Collections.Generic;
using System.Text;
using WSC.Shared.Contracts.Dtos;
using WSC.Store.Domain.Entities;

namespace WSC.Store.Application.Interfaces.ServiceInterfaces
{
    public interface IOrderItemsService
    {
        Task<IEnumerable<OrderItemResponseDto>> GetAllOrderItemsAsync(CancellationToken ct);
        Task<OrderItemResponseDto> GetItemByIdAsync(int orderItemId, CancellationToken ct);
        Task<int> CreateOrderItemAsync(OrderItems items, CancellationToken ct);
        Task<bool> UpdateOrderItemAsync(OrderItems items, CancellationToken ct);
        Task<bool> DeleteOrderItemAsync(int orderItemId, CancellationToken ct);
    }
}
