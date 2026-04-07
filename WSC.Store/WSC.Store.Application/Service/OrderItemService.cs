using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WSC.Shared.Contracts.Dtos;
using WSC.Store.Application.Dtos;
using WSC.Store.Application.Interfaces.RepositoryInterfaces;
using WSC.Store.Application.Interfaces.ServiceInterfaces;
using WSC.Store.Domain.Entities;

namespace WSC.Store.Application.Service
{
    public class OrderItemService : IOrderItemsService
    {
        private readonly IOrderItemsRepository _itemsRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly IProductRepository _productRepo;
        private readonly IMapper _mapper;

        public OrderItemService(IOrderItemsRepository itemsRepo,
                                IOrderRepository orderRepo,
                                IProductRepository productRepo,
                                IMapper mapper)
        {
            _itemsRepo = itemsRepo;
            _orderRepo = orderRepo;
            _productRepo = productRepo;
            _mapper = mapper;
        }
        public Task<int> CreateOrderItemAsync(CreateItemsDto items, CancellationToken ct)
        {
            throw new NotImplementedException();

        }

        public Task<bool> DeleteOrderItemAsync(int orderItemId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderItemResponseDto>> GetAllOrderItemsAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<OrderItemResponseDto> GetItemByIdAsync(int orderItemId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateOrderItemAsync(OrderItems items, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
