using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
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
            CreateMap<CreateOpportunityDto, Domain.Entities.Opportunity>();
            CreateMap<UpdateOpportunityDto, Domain.Entities.Opportunity>();
        }
    }
}
