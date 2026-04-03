using AutoMapper;
using WSC.CRM.Application.Dtos;
using WSC.CRM.Domain.Entities;
using WSC.Shared.Contracts.Dtos;

namespace WSC.CRM.Application.Mappings
{
    public class OpportunityProfile : Profile
    {
        public OpportunityProfile()
        {
            CreateMap<Opportunity, OpportunityResponseDto>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.CxName));
            CreateMap<CreateOpportunityDto, Opportunity>();
            CreateMap<UpdateOpportunityDto, Opportunity>();
        }
    }
}
