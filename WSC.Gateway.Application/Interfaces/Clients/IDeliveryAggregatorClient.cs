using System;
using System.Collections.Generic;
using System.Text;
using WSC.Shared.Contracts.Dtos.DeliveryLayer;

namespace WSC.Gateway.Application.Interfaces.Clients
{
    public interface IDeliveryAggregatorClient
    {
        Task<IEnumerable<OrderDeliveryResponseDto>?> GetAllDeliveriesAsync(CancellationToken ct);
        Task<IEnumerable<DeliveryAgentResponseDto>> GetAllAgentsAsync(CancellationToken ct);
    }
}
