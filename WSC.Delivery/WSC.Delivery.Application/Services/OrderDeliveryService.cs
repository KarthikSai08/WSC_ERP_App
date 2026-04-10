using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Linq;
using WSC.Delivery.Application.Dtos;
using WSC.Delivery.Application.Interfaces.RepositoryInterfaces;
using WSC.Delivery.Application.Interfaces.ServiceInterfaces;
using WSC.Delivery.Domain.Entities;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.DeliveryLayer;
using WSC.Shared.Contracts.Exceptions;
using WSC.Shared.Contracts.Interfaces.CRMClients;
using WSC.Shared.Contracts.Interfaces.StoreClients;

namespace WSC.Delivery.Application.Services
{
    public class OrderDeliveryService : IOrderDeliveryService
    {
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly ICustomerClient _cstClient;
        private readonly IOrderClient _orderClient;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderDeliveryService> _logger;

        public OrderDeliveryService(
            IDeliveryRepository deliveryRepository,
            IMapper mapper,
            ILogger<OrderDeliveryService> logger,ICustomerClient cstClient, IOrderClient orderClient)
        {
            _deliveryRepository = deliveryRepository ?? throw new ArgumentNullException(nameof(deliveryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cstClient = cstClient ?? throw new ArgumentNullException(nameof(cstClient));
            _orderClient = orderClient ?? throw new ArgumentNullException(nameof(orderClient));
        }

        public async Task<ApiResponse<int>> CreateOrderDeliveryAsync(CreateOrderDeliveryDto dto, CancellationToken ct)
        {
            if (dto == null)
                return ApiResponse<int>.Failed("Invalid delivery data.");
            try
            {
<<<<<<< HEAD
<<<<<<< Updated upstream
=======
                var orderExists = await _orderClient.GetByOrderIdAsync(dto.OrderId, ct);
                var customerExists = await _cstClient.GetCustomerByIdAsync(dto.CustomerId, ct);
                if(orderExists == null)
                {
                    return ApiResponse<int>.Failed("Order not found. Please provide a valid Order Id ");
                }

                if (customerExists == null)
                {
                    _logger.LogWarning("Customer not found. ID: {CustomerId}", dto.CustomerId);
                    return ApiResponse<int>.Failed("Customer not found. Please provide a valid customer ID.");
                }

>>>>>>> feature(delivery)/delivery-module
                if (dto == null)
                    return ApiResponse<int>.Failed("Invalid delivery data.");
=======
                var orderExists = await _orderClient.GetByOrderIdAsync(dto.OrderId, ct);
                if (orderExists == null)
                {
                    return ApiResponse<int>.Failed("Order not found. Please provide a valid Order Id ");
                }

                var customerExists = await _cstClient.GetCustomerByIdAsync(dto.CustomerId, ct);
               

                if (customerExists == null)
                {
                    _logger.LogWarning("Customer not found. ID: {CustomerId}", dto.CustomerId);
                    return ApiResponse<int>.Failed("Customer not found. Please provide a valid customer ID.");
                }
>>>>>>> Stashed changes

                _logger.LogInformation("Creating new order delivery for order ID: {OrderId}", dto.OrderId);

                var delivery = _mapper.Map<OrderDelivery>(dto);
                var deliveryId = await _deliveryRepository.CreateDeliveryAsync(delivery, ct);

                _logger.LogInformation("Order delivery created successfully. ID: {DeliveryId}", deliveryId);
                return ApiResponse<int>.Ok(deliveryId, "Order delivery created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order delivery");
                return ApiResponse<int>.Failed("Failed to create order delivery. Please try again later.");
            }
        }

        public async Task<ApiResponse<IEnumerable<OrderDeliveryResponseDto>>> GetAllDeliveriesAsync(CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Fetching all order deliveries");

                var deliveries = await _deliveryRepository.GetAllDeliveriesAsync(ct);

                if (deliveries == null || !deliveries.Any())
                {
                    _logger.LogInformation("No order deliveries found");
                    return ApiResponse<IEnumerable<OrderDeliveryResponseDto>>.Ok(
                        new List<OrderDeliveryResponseDto>(),
                        "No order deliveries found.");
                }

                var mappedDeliveries = _mapper.Map<IEnumerable<OrderDeliveryResponseDto>>(deliveries);
                return ApiResponse<IEnumerable<OrderDeliveryResponseDto>>.Ok(
                    mappedDeliveries,
                    "Order deliveries retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all order deliveries");
                return ApiResponse<IEnumerable<OrderDeliveryResponseDto>>.Failed(
                    "Failed to retrieve order deliveries. Please try again later.");
            }
        }

        public async Task<ApiResponse<OrderDeliveryResponseDto>> GetDeliveryByIdAsync(int deliveryId, CancellationToken ct)
        {
            try
            {
                if (deliveryId <= 0)
                    return ApiResponse<OrderDeliveryResponseDto>.Failed("Invalid delivery ID.");

                _logger.LogInformation("Fetching order delivery with ID: {DeliveryId}", deliveryId);

                var delivery = await _deliveryRepository.GetByDeliveryIdAsync(deliveryId, ct);

                if (delivery == null)
                {
                    _logger.LogWarning("Order delivery not found. ID: {DeliveryId}", deliveryId);
                    return ApiResponse<OrderDeliveryResponseDto>.Failed("Order delivery not found.");
                }

                var mappedDelivery = _mapper.Map<OrderDeliveryResponseDto>(delivery);
                return ApiResponse<OrderDeliveryResponseDto>.Ok(mappedDelivery, "Order delivery retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching order delivery {DeliveryId}", deliveryId);
                return ApiResponse<OrderDeliveryResponseDto>.Failed(
                    "Failed to retrieve order delivery. Please try again later.");
            }
        }

        public async Task<ApiResponse<bool>> UpdateOrderDeliveryAsync(UpdateOrderDeliveryDto dto, CancellationToken ct)
        {
            try
            {
                if (dto == null || dto.DeliveryId <= 0)
                    return ApiResponse<bool>.Failed("Invalid delivery data.");

                _logger.LogInformation("Updating order delivery with ID: {DeliveryId}", dto.DeliveryId);

                var existingDelivery = await _deliveryRepository.GetDeliveryEntityByIdAsync(dto.DeliveryId, ct);
                if (existingDelivery == null)
                {
                    _logger.LogWarning("Order delivery not found for update. ID: {DeliveryId}", dto.DeliveryId);
                    throw new NotFoundException("Order Delivery", dto.DeliveryId);
                }

                _mapper.Map(dto, existingDelivery);
                var updated = await _deliveryRepository.UpdateDeliveryDetailsAsync(existingDelivery, ct);

                if (!updated)
                    return ApiResponse<bool>.Failed("Failed to update order delivery.");

                _logger.LogInformation("Order delivery updated successfully. ID: {DeliveryId}", dto.DeliveryId);
                return ApiResponse<bool>.Ok(true, "Order delivery updated successfully");
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Order delivery not found");
                return ApiResponse<bool>.Failed("Order delivery not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order delivery {DeliveryId}", dto?.DeliveryId);
                return ApiResponse<bool>.Failed("Failed to update order delivery. Please try again later.");
            }
        }

        public async Task<ApiResponse<bool>> DeleteDeliveryAsync(int deliveryId, CancellationToken ct)
        {
            try
            {
                if (deliveryId <= 0)
                    return ApiResponse<bool>.Failed("Invalid delivery ID.");

                _logger.LogInformation("Deleting order delivery with ID: {DeliveryId}", deliveryId);

                var existingDelivery = await _deliveryRepository.GetDeliveryEntityByIdAsync(deliveryId, ct);
                if (existingDelivery == null)
                    return ApiResponse<bool>.Failed("Order delivery not found or already deleted.");

                var deleted = await _deliveryRepository.UpdateDeliveryDetailsAsync(existingDelivery, ct);

                if (!deleted)
                    return ApiResponse<bool>.Failed("Failed to delete order delivery.");

                _logger.LogInformation("Order delivery deleted successfully. ID: {DeliveryId}", deliveryId);
                return ApiResponse<bool>.Ok(true, "Order delivery deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order delivery {DeliveryId}", deliveryId);
                return ApiResponse<bool>.Failed("Failed to delete order delivery. Please try again later.");
            }
        }

        public async Task<ApiResponse<IEnumerable<OrderDeliveryResponseDto>>> GetDeliveriesByStatusAsync(string status, CancellationToken ct)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(status))
                    return ApiResponse<IEnumerable<OrderDeliveryResponseDto>>.Failed("Invalid status.");

                _logger.LogInformation("Fetching deliveries by status: {Status}", status);

                var deliveries = await _deliveryRepository.GetByDeliveryStatusAsync(status, ct);

                if (deliveries == null || !deliveries.Any())
                {
                    _logger.LogInformation("No deliveries found with status: {Status}", status);
                    return ApiResponse<IEnumerable<OrderDeliveryResponseDto>>.Ok(
                        new List<OrderDeliveryResponseDto>(),
                        $"No deliveries found with status: {status}");
                }

                var mappedDeliveries = _mapper.Map<IEnumerable<OrderDeliveryResponseDto>>(deliveries);
                return ApiResponse<IEnumerable<OrderDeliveryResponseDto>>.Ok(
                    mappedDeliveries,
                    "Deliveries retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching deliveries by status: {Status}", status);
                return ApiResponse<IEnumerable<OrderDeliveryResponseDto>>.Failed(
                    "Failed to retrieve deliveries. Please try again later.");
            }
        }

        public async Task<ApiResponse<IEnumerable<OrderDeliveryResponseDto>>> GetDeliveriesByAgentIdAsync(int agentId, CancellationToken ct)
        {
            try
            {
                if (agentId <= 0)
                    return ApiResponse<IEnumerable<OrderDeliveryResponseDto>>.Failed("Invalid agent ID.");

                _logger.LogInformation("Fetching deliveries for agent ID: {AgentId}", agentId);

                var deliveries = await _deliveryRepository.GetAllDeliveriesAsync(ct);
                var agentDeliveries = deliveries?.Where(d => d.AssignedAgentId == agentId).ToList();

                if (agentDeliveries == null || !agentDeliveries.Any())
                {
                    _logger.LogInformation("No deliveries found for agent ID: {AgentId}", agentId);
                    return ApiResponse<IEnumerable<OrderDeliveryResponseDto>>.Ok(
                        new List<OrderDeliveryResponseDto>(),
                        "No deliveries found for this agent.");
                }

                var mappedDeliveries = _mapper.Map<IEnumerable<OrderDeliveryResponseDto>>(agentDeliveries);
                return ApiResponse<IEnumerable<OrderDeliveryResponseDto>>.Ok(
                    mappedDeliveries,
                    "Agent deliveries retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching deliveries for agent ID: {AgentId}", agentId);
                return ApiResponse<IEnumerable<OrderDeliveryResponseDto>>.Failed(
                    "Failed to retrieve agent deliveries. Please try again later.");
            }
        }
    }
}
