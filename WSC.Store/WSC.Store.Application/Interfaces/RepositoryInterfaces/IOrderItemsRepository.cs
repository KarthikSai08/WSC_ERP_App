using System.Data;
using WSC.Shared.Contracts.Dtos.StoreLayer;
using WSC.Store.Domain.Entities;

namespace WSC.Store.Application.Interfaces.RepositoryInterfaces
{
    public interface IOrderItemsRepository
    {
        Task<IEnumerable<OrderItemResponseDto>> GetAllOrderItemsAsync(CancellationToken ct);

        Task<OrderItemResponseDto> GetItemByIdAsync(int orderItemId, CancellationToken ct);
        Task<OrderItems> GetOrderItemEntityByIdAsync(int orderId, CancellationToken ct);
        Task<int> CreateOrderItemAsync(OrderItems items, IDbTransaction transaction, CancellationToken ct);
        Task<bool> UpdateOrderItemAsync(OrderItems items, CancellationToken ct);
        Task<bool> DeleteOrderItemAsync(int orderItemId, CancellationToken ct);
    }
}
