using AutoMapper;
using Microsoft.Extensions.Logging;
using WSC.Delivery.Application.Dtos;
using WSC.Delivery.Application.Interfaces.RepositoryInterfaces;
using WSC.Delivery.Application.Interfaces.ServiceInterfaces;
using WSC.Delivery.Domain.Entities;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.DeliveryLayer;
using WSC.Shared.Contracts.Exceptions;

namespace WSC.Delivery.Application.Services
{
    public sealed class DeliveryAgentService : IDeliveryAgentService
    {
        private readonly IDeliveryAgentRepository _agentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeliveryAgentService> _logger;

        public DeliveryAgentService(
            IDeliveryAgentRepository agentRepository,
            IMapper mapper,
            ILogger<DeliveryAgentService> logger)
        {
            _agentRepository = agentRepository ?? throw new ArgumentNullException(nameof(agentRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ApiResponse<int>> CreateDeliveryAgentAsync(CreateDeliveryAgentDto dto, CancellationToken ct)
        {
            try
            {
                if (dto == null)
                    return ApiResponse<int>.Failed("Invalid delivery agent data.");

                _logger.LogInformation("Creating new delivery agent: {Name}", dto.AgentName);

                var agent = _mapper.Map<DeliveryAgent>(dto);
                var agentId = await _agentRepository.CreateDeliveryAgentAsync(agent, ct);

                _logger.LogInformation("Delivery agent created successfully. ID: {AgentId}", agentId);
                return ApiResponse<int>.Ok(agentId, "Delivery agent created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating delivery agent");
                return ApiResponse<int>.Failed("Failed to create delivery agent. Please try again later.");
            }
        }

        public async Task<ApiResponse<IEnumerable<DeliveryAgentResponseDto>>> GetAllAgentsAsync(CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Fetching all delivery agents");

                var agents = await _agentRepository.GetAllAgentsAsync(ct);

                if (agents == null || !agents.Any())
                {
                    _logger.LogInformation("No delivery agents found");
                    return ApiResponse<IEnumerable<DeliveryAgentResponseDto>>.Ok(
                        new List<DeliveryAgentResponseDto>(),
                        "No delivery agents found.");
                }

                return ApiResponse<IEnumerable<DeliveryAgentResponseDto>>.Ok(
                    agents,
                    "Delivery agents retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all delivery agents");
                return ApiResponse<IEnumerable<DeliveryAgentResponseDto>>.Failed(
                    "Failed to retrieve delivery agents. Please try again later.");
            }
        }

        public async Task<ApiResponse<DeliveryAgentResponseDto>> GetAgentByIdAsync(int agentId, CancellationToken ct)
        {
            try
            {
                if (agentId <= 0)
                    return ApiResponse<DeliveryAgentResponseDto>.Failed("Invalid agent ID.");

                _logger.LogInformation("Fetching delivery agent with ID: {AgentId}", agentId);

                var agent = await _agentRepository.GetAgentByIdAsync(agentId, ct);

                if (agent == null)
                {
                    _logger.LogWarning("Delivery agent not found. ID: {AgentId}", agentId);
                    return ApiResponse<DeliveryAgentResponseDto>.Failed("Delivery agent not found.");
                }

                return ApiResponse<DeliveryAgentResponseDto>.Ok(agent, "Delivery agent retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching delivery agent {AgentId}", agentId);
                return ApiResponse<DeliveryAgentResponseDto>.Failed(
                    "Failed to retrieve delivery agent. Please try again later.");
            }
        }

        public async Task<ApiResponse<bool>> UpdateDeliveryAgentAsync(UpdateDeliveryAgentDto dto, CancellationToken ct)
        {
            try
            {
                if (dto == null || dto.AgentId <= 0)
                    return ApiResponse<bool>.Failed("Invalid agent data.");

                _logger.LogInformation("Updating delivery agent with ID: {AgentId}", dto.AgentId);

                var existingAgent = await _agentRepository.GetAgentEntityByIdAsync(dto.AgentId, ct);
                if (existingAgent == null)
                {
                    _logger.LogWarning("Delivery agent not found for update. ID: {AgentId}", dto.AgentId);
                    throw new NotFoundException("Delivery Agent", dto.AgentId);
                }

                _mapper.Map(dto, existingAgent);
                var updated = await _agentRepository.UpdateAgentAsync(existingAgent, ct);

                if (!updated)
                    return ApiResponse<bool>.Failed("Failed to update delivery agent.");

                _logger.LogInformation("Delivery agent updated successfully. ID: {AgentId}", dto.AgentId);
                return ApiResponse<bool>.Ok(true, "Delivery agent updated successfully");
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Delivery agent not found");
                return ApiResponse<bool>.Failed($"Delivery agent not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating delivery agent {AgentId}", dto?.AgentId);
                return ApiResponse<bool>.Failed("Failed to update delivery agent. Please try again later.");
            }
        }

        public async Task<ApiResponse<bool>> DeleteAgentAsync(int agentId, CancellationToken ct)
        {
            try
            {
                if (agentId <= 0)
                    return ApiResponse<bool>.Failed("Invalid agent ID.");

                _logger.LogInformation("Deleting delivery agent with ID: {AgentId}", agentId);

                var deleted = await _agentRepository.DeleteAgentByIdAsync(agentId, ct);

                if (!deleted)
                    return ApiResponse<bool>.Failed("Delivery agent not found or already deleted.");

                _logger.LogInformation("Delivery agent deleted successfully. ID: {AgentId}", agentId);
                return ApiResponse<bool>.Ok(true, "Delivery agent deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting delivery agent {AgentId}", agentId);
                return ApiResponse<bool>.Failed("Failed to delete delivery agent. Please try again later.");
            }
        }

        public async Task<ApiResponse<IEnumerable<DeliveryAgentResponseDto>>> GetAvailableAgentsAsync(CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Fetching available delivery agents");

                var agents = await _agentRepository.GetAvailableAgentsAsync(ct);

                if (agents == null || !agents.Any())
                {
                    _logger.LogInformation("No available delivery agents found");
                    return ApiResponse<IEnumerable<DeliveryAgentResponseDto>>.Ok(
                        new List<DeliveryAgentResponseDto>(),
                        "No available delivery agents found.");
                }

                var mappedAgents = _mapper.Map<IEnumerable<DeliveryAgentResponseDto>>(agents);
                return ApiResponse<IEnumerable<DeliveryAgentResponseDto>>.Ok(
                    mappedAgents,
                    "Available delivery agents retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching available delivery agents");
                return ApiResponse<IEnumerable<DeliveryAgentResponseDto>>.Failed(
                    "Failed to retrieve available delivery agents. Please try again later.");
            }
        }
    }
}
