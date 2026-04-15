using AutoMapper;
using WSC.CRM.Application.Dtos;
using WSC.CRM.Domain.Entities;
using WSC.Shared.Contracts.Dtos.CRMLayer;

namespace WSC.CRM.Application.Mappings
{
    public class OpportunityProfile : Profile
    {
        public OpportunityProfile()
        {
            CreateMap<Opportunity, OpportunityResponseDto>();
            CreateMap<CreateOpportunityDto, Opportunity>();
            CreateMap<UpdateOpportunityDto, Opportunity>();
        }
    }
}
