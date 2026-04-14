using AutoMapper;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.StoreLayer;
using WSC.Shared.Contracts.Exceptions;
using WSC.Shared.Contracts.Interfaces.CRMClients;
using WSC.Store.Application.Dtos;
using WSC.Store.Application.Interfaces;
using WSC.Store.Application.Interfaces.RepositoryInterfaces;
using WSC.Store.Application.Interfaces.ServiceInterfaces;
using WSC.Store.Domain.Entities;

namespace WSC.Store.Application.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly ICustomerClient _cstService;
        private readonly IRedisCacheService _cache;
        private readonly IMapper _mapper;


        public OrderService(IOrderRepository orderRepo, ICustomerClient cstService, IMapper mapper, IRedisCacheService cache)
        {
            _orderRepo = orderRepo;
            _cstService = cstService;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<ApiResponse<int>> CreateOrderAsync(CreateOrderDto dto, CancellationToken ct)
        {

            var customer = await _cstService.GetCustomerByIdAsync(dto.CustomerId, ct);
            if (customer == null)
                throw new NotFoundException("Customer", dto.CustomerId);

            var data = _mapper.Map<Order>(dto);
            var createdOrder = await _orderRepo.CreateOrderAsync(data, ct);

            if (createdOrder == null)
                return ApiResponse<int>.Failed("Failed To Create Order");
            return ApiResponse<int>.Ok(createdOrder, "Order created successfully");
        }

        public async Task<ApiResponse<bool>> DeleteOrderAsync(int id, CancellationToken ct)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid order ID", nameof(id));

            var deleted = await _orderRepo.DeleteOrderAsync(id, ct);

            if (!deleted)
                return ApiResponse<bool>.Failed("Failed to delete order. Order may not exist.");
            return ApiResponse<bool>.Ok(true, "Order deleted successfully");
        }

        public async Task<ApiResponse<IEnumerable<OrderResponseDto>>> GetAllOrdersAsync(CancellationToken ct)
        {
            var cacheKey = "orders:all";

            var cached = await _cache.GetAsync<IEnumerable<OrderResponseDto>>(cacheKey);

            if (cached != null)
                return ApiResponse<IEnumerable<OrderResponseDto>>.Ok(cached, "Orders retrieved from cache successfully");

            var orders = await _orderRepo.GetAllOrdersAsync(ct);
            if (orders == null || !orders.Any())
                return ApiResponse<IEnumerable<OrderResponseDto>>.Ok(new List<OrderResponseDto>(), "No orders found.");
            var mappedOrders = _mapper.Map<IEnumerable<OrderResponseDto>>(orders);

            if (orders != null)
                await _cache.SetAsync(cacheKey, mappedOrders, TimeSpan.FromMinutes(10));
            return ApiResponse<IEnumerable<OrderResponseDto>>.Ok(mappedOrders.ToList(), "Orders retrieved successfully");

        }

        public async Task<ApiResponse<OrderResponseDto>> GetByIdAsync(int id, CancellationToken ct)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid order ID", nameof(id));

            var order = await _orderRepo.GetOrderByIdAsync(id, ct);
            if (order == null)
                return ApiResponse<OrderResponseDto>.Failed("Order not found.");

            return ApiResponse<OrderResponseDto>.Ok(order, "Order retrieved successfully");
        }

        public async Task<ApiResponse<bool>> UpdateOrderAsync(UpdateOrderDto dto, CancellationToken ct)
        {
            var order = await _orderRepo.GetOrderEntityByIdAsync(dto.OrderId, ct);
            if (order == null)
                throw new NotFoundException("Order", dto.OrderId);

            _mapper.Map(dto, order);
            var updated = await _orderRepo.UpdateOrderAsync(order, ct);

            if (!updated)
                return ApiResponse<bool>.Failed("Failed to update order. Order may not exist.");
            return ApiResponse<bool>.Ok(true, "Order updated successfully");
        }
    }
}
