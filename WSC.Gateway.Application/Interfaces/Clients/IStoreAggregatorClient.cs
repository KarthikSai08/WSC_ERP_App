using System;
using System.Collections.Generic;
using System.Text;
using WSC.Shared.Contracts.Dtos.StoreLayer;

namespace WSC.Gateway.Application.Interfaces.Clients
{
    public interface IStoreAggregatorClient
    {
        Task<IEnumerable<OrderResponseDto>?> GetAllOrdersAsync(CancellationToken ct);
         Task<IEnumerable<ProductResponseDto>?> GetAllProductsAsync(CancellationToken ct);
    }
}
