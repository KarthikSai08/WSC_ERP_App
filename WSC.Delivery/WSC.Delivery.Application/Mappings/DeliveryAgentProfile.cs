using AutoMapper;
using WSC.Delivery.Domain.Entities;
using WSC.Shared.Contracts.Dtos.DeliveryLayer;

namespace WSC.Delivery.Application.Mappings
{
    public class DeliveryAgentProfile : Profile
    {
        public DeliveryAgentProfile()
        {
            CreateMap<DeliveryAgent, DeliveryAgentResponseDto>();
            
            CreateMap<CreateDeliveryAgentDto, DeliveryAgent>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
            
            CreateMap<UpdateDeliveryAgentDto, DeliveryAgent>()
                .ForMember(dest => dest.DeliveryAgentId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        }
    }
}
