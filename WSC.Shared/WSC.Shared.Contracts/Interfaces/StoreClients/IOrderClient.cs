using System;
using System.Collections.Generic;
using System.Text;
using WSC.Shared.Contracts.Dtos.StoreLayer;

namespace WSC.Shared.Contracts.Interfaces.StoreClients
{
    public interface IOrderClient
    {
        Task<OrderResponseDto?> GetByOrderIdAsync(int orderId, CancellationToken ct);
    }
}
