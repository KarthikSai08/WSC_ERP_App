using AutoMapper;
using WSC.Delivery.Application.Dtos;
using WSC.Delivery.Domain.Entities;
using WSC.Shared.Contracts.Dtos.DeliveryLayer;

namespace WSC.Delivery.Application.Mappings
{
    public class OrderDeliveryProfile : Profile
    {
        public OrderDeliveryProfile()
        {
            CreateMap<OrderDelivery, OrderDeliveryResponseDto>();

            CreateMap<CreateOrderDeliveryDto, OrderDelivery>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => Domain.Enums.DeliveryStatus.Pending))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true));

            CreateMap<UpdateOrderDeliveryDto, OrderDelivery>()
                .ForMember(dest => dest.DeliveryId, opt => opt.Ignore())
                .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        }
    }
}
