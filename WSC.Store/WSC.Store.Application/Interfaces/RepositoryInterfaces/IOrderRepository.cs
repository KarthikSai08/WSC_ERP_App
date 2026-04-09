using WSC.Shared.Contracts.Dtos.StoreLayer;
using WSC.Store.Domain.Entities;

namespace WSC.Store.Application.Interfaces.RepositoryInterfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync(CancellationToken ct);

        Task<OrderResponseDto> GetOrderByIdAsync(int id, CancellationToken ct);
        Task<Order> GetOrderEntityByIdAsync(int id, CancellationToken ct);
        Task<int> CreateOrderAsync(Order order, CancellationToken ct);
        Task<bool> UpdateOrderAsync(Order order, CancellationToken ct);
        Task<bool> DeleteOrderAsync(int id, CancellationToken ct);

    }
}
