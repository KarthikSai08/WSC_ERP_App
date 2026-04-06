using System;
using System.Collections.Generic;
using System.Text;
using WSC.Store.Domain.Entities;

namespace WSC.Store.Application.Interfaces.RepositoryInterfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync(CancellationToken ct);

        Task<Order> GetOrderByIdAsync(int id, CancellationToken ct);
        Task<int> CreateOrderAsync(Order order, CancellationToken ct);
        Task<bool> UpdateOrderAsync(Order order, CancellationToken ct);
        Task<bool> DeleteOrderAsync(int id, CancellationToken ct);

    }
}
