using AutoMapper;
using WSC.Delivery.Domain.Entities;
using WSC.Shared.Contracts.Dtos.DeliveryLayer;

namespace WSC.Delivery.Application.Mappings
{
    public class DeliveryItemProfile : Profile
    {
        public DeliveryItemProfile()
        {
            CreateMap<DeliveryItem, DeliveryItemResponseDto>();
            
            CreateMap<CreateDeliveryItemDto, DeliveryItem>();
            
            CreateMap<UpdateDeliveryItemDto, DeliveryItem>()
                .ForMember(dest => dest.DeliveryItemId, opt => opt.Ignore())
                .ForMember(dest => dest.DeliveryId, opt => opt.Ignore());
        }
    }
}
