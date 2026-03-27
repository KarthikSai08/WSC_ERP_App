using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WSC.CRM.Domain.Entities;
using WSC.Shared.Contracts.Dtos;

namespace WSC.CRM.Application.Mappings
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile() { 
            CreateMap<Customer, CustomerResponseDto>();
        }
    }
}
