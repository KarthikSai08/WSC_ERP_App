using AutoMapper;
using WSC.Shared.Contracts.Dtos.StoreLayer;
using WSC.Store.Application.Dtos;
using WSC.Store.Domain.Entities;

namespace WSC.Store.Application.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderResponseDto>();

            CreateMap<UpdateOrderDto, Order>();
            CreateMap<CreateOrderDto, Order>();
        }
    }
}
