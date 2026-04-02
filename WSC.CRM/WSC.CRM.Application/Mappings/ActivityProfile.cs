using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WSC.CRM.Application.Dtos;
using WSC.CRM.Domain.Entities;
using WSC.Shared.Contracts.Dtos;

namespace WSC.CRM.Application.Mappings
{
    public class ActivityProfile : Profile
    {
        public ActivityProfile() 
        {
            CreateMap<Activity, ActivityResponseDto>()
                .ForMember(dest => dest.LeadName, opt => opt.MapFrom(src => src.Lead.LeadName));
             CreateMap<CreateActivityDto, Activity>();
             CreateMap<UpdateActivityDto, Activity>();
        }
    }
}
