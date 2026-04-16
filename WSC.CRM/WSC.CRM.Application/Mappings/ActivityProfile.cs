using AutoMapper;
using WSC.CRM.Application.Dtos;
using WSC.CRM.Domain.Entities;
using WSC.Shared.Contracts.Dtos.CRMLayer;

namespace WSC.CRM.Application.Mappings
{
    public class ActivityProfile : Profile
    {
        public ActivityProfile()
        {
            CreateMap<Activity, ActivityResponseDto>();
            CreateMap<CreateActivityDto, Activity>();
            CreateMap<UpdateActivityDto, Activity>();
        }
    }
}
