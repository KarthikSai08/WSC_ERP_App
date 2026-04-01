using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WSC.CRM.Application.Dtos;
using WSC.CRM.Domain.Entities;

namespace WSC.CRM.Application.Mappings
{
    public class LeadProfile : Profile
    {
        public LeadProfile()
        {
            CreateMap<CreateLeadDto, Lead> ();
            CreateMap<UpdateLeadDto, Lead>();
        }
    }
}
