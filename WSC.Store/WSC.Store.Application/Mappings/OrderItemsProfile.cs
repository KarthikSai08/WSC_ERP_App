using AutoMapper;
using WSC.Shared.Contracts.Dtos;
using WSC.Store.Application.Dtos;
using WSC.Store.Domain.Entities;

namespace WSC.Store.Application.Mappings
{
    public class OrderItemsProfile : Profile
    {
        public OrderItemsProfile()
        {
            CreateMap<OrderItems, OrderItemResponseDto>();

            CreateMap<CreateItemsDto, OrderItems>()
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Quantity * src.UnitPrice));
            CreateMap<UpdateItemsDto, OrderItems>()
               .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Quantity * src.UnitPrice));
        }
    }
}
