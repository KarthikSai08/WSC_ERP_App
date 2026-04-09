using System;
using System.Collections.Generic;
using System.Text;
using WSC.Delivery.Domain.Entities;
using WSC.Shared.Contracts.Dtos.DeliveryLayer;

namespace WSC.Delivery.Application.Interfaces.RepositoryInterfaces
{
    public interface IDeliveryAgentRepository
    {
        Task<int> CreateDeliveryAgentAsync(DeliveryAgent agent, CancellationToken ct);
        Task<IEnumerable<DeliveryAgentResponseDto>> GetAllAgentsAsync(CancellationToken ct);
        Task<DeliveryAgentResponseDto> GetAgentByIdAsync(int agentId, CancellationToken ct);
        Task<DeliveryAgent> GetAgentEntityByIdAsync(int agentId, CancellationToken ct);
        Task<IEnumerable<DeliveryAgent>> GetAvailableAgentsAsync(CancellationToken ct);
        Task<bool> UpdateAgentAsync(DeliveryAgent agent, CancellationToken ct);
        Task<bool> DeleteAgentByIdAsync(int agentId, CancellationToken ct);
    }
}
