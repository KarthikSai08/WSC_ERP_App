using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WSC.CRM.Application.Interfaces.Repository;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.StoreLayer;
using WSC.Shared.Contracts.Exceptions;
using WSC.Store.Application.Dtos;
using WSC.Store.Application.Interfaces.RepositoryInterfaces;
using WSC.Store.Application.Interfaces.ServiceInterfaces;
using WSC.Store.Domain.Entities;

namespace WSC.Store.Application.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly ICustomerRepository _cstRepo;
        private readonly IMapper _mapper;


        public OrderService(IOrderRepository orderRepo, ICustomerRepository cstRepo, IMapper mapper)
        {
            _orderRepo = orderRepo;
            _cstRepo = cstRepo;
            _mapper = mapper;
        }

        public async Task<ApiResponse<int>> CreateOrderAsync(CreateOrderDto dto, CancellationToken ct)
        {
         
            var customer =await _cstRepo.GetCustomerByIdAsync(dto.CustomerId, ct);
            if(customer == null)
                throw new NotFoundException("Customer", dto.CustomerId);

            var data = _mapper.Map<Order>(dto);
            var createdOrder = await _orderRepo.CreateOrderAsync(data, ct);

            return ApiResponse<int>.Ok(createdOrder, "Order created successfully");
        }

        public async Task<ApiResponse<bool>> DeleteOrderAsync(int id, CancellationToken ct)
        {
            if(id <= 0)
                throw new ArgumentException("Invalid order ID", nameof(id));
             
            var deleted = await _orderRepo.DeleteOrderAsync(id, ct);

            if(!deleted)
                return ApiResponse<bool>.Failed("Failed to delete order. Order may not exist.");
            return ApiResponse<bool>.Ok(true, "Order deleted successfully");
        }

        public async Task<ApiResponse<IEnumerable<OrderResponseDto>>> GetAllOrdersAsync(CancellationToken ct)
        {
            var orders =await _orderRepo.GetAllOrdersAsync(ct);
            if(orders == null || !orders.Any())
                return ApiResponse<IEnumerable<OrderResponseDto>>.Ok(new List<OrderResponseDto>(), "No orders found.");

            var mappedOrders = _mapper.Map<IEnumerable<OrderResponseDto>>(orders);
            return ApiResponse<IEnumerable<OrderResponseDto>>.Ok(mappedOrders.ToList(), "Orders retrieved successfully");

        }

        public async Task<ApiResponse<OrderResponseDto>> GetByIdAsync(int id, CancellationToken ct)
        {
            if(id <= 0)
                throw new ArgumentException("Invalid order ID", nameof(id));
             
            var order = await _orderRepo.GetOrderByIdAsync(id, ct);
            if(order == null)
                return ApiResponse<OrderResponseDto>.Failed("Order not found.");

            var mappedOrder = _mapper.Map<OrderResponseDto>(order);

            return ApiResponse<OrderResponseDto>.Ok(mappedOrder, "Order retrieved successfully");
        }

        public async Task<ApiResponse<bool>> UpdateOrderAsync(UpdateOrderDto dto, CancellationToken ct)
        {
           var order = await _orderRepo.GetOrderByIdAsync(dto.OrderId, ct);
           if(order == null)
               throw new NotFoundException("Order", dto.OrderId);

           _mapper.Map(dto, order);
           var updated = await _orderRepo.UpdateOrderAsync(order, ct);

            if(!updated)
                return ApiResponse<bool>.Failed("Failed to update order. Order may not exist.");

            return ApiResponse<bool>.Ok(true, "Order updated successfully");


        }
    }
}
