using AutoMapper;
using WSC.Delivery.Domain.Entities;
using WSC.Shared.Contracts.Dtos.DeliveryLayer;

namespace WSC.Delivery.Application.Mappings
{
    public class DeliveryTrackingProfile : Profile
    {
        public DeliveryTrackingProfile()
        {
            CreateMap<DeliveryTracking, DeliveryTrackingResponseDto>();
            
            CreateMap<CreateDeliveryTrackingDto, DeliveryTracking>()
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(_ => DateTime.UtcNow));
        }
    }
}
