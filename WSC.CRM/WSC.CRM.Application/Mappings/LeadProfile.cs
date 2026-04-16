using AutoMapper;
using WSC.CRM.Application.Dtos;
using WSC.CRM.Domain.Entities;
using WSC.Shared.Contracts.Dtos.CRMLayer;

namespace WSC.CRM.Application.Mappings
{
    public class LeadProfile : Profile
    {
        public LeadProfile()
        {
            CreateMap<Lead, LeadResponseDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<CreateLeadDto, Lead>();
            CreateMap<UpdateLeadDto, Lead>();
        }
    }
}
