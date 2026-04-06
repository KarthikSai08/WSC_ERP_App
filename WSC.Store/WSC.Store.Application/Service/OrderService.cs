using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WSC.CRM.Application.Interfaces.Repository;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.StoreLayer;
using WSC.Store.Application.Dtos;
using WSC.Store.Application.Interfaces.RepositoryInterfaces;
using WSC.Store.Application.Interfaces.ServiceInterfaces;

namespace WSC.Store.Application.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly ICustomerRepository _cstRepo;
        private readonly IProductRepository _prdRepo;
        private readonly IMapper _mapper;


        public OrderService(IOrderRepository orderRepo, ICustomerRepository cstRepo,
                            IProductRepository prdRepo, IMapper mapper)
        {
            _orderRepo = orderRepo;
            _cstRepo = cstRepo;
            _prdRepo = prdRepo;
            _mapper = mapper;
        }

        public Task<ApiResponse<int>> CreateOrderAsync(CreateOrderDto dto, CancellationToken ct)
        {
            
        }

        public Task<ApiResponse<bool>> DeleteOrderAsync(int id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<IEnumerable<OrderResponseDto>>> GetAllOrdersAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<OrderResponseDto>> GetByIdAsync(int id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> UpdateOrderAsync(UpdateOrderDto dto, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
