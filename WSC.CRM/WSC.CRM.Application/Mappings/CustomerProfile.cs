using AutoMapper;
using WSC.CRM.Application.Dtos;
using WSC.CRM.Domain.Entities;
using WSC.Shared.Contracts.Dtos;
using WSC.Shared.Contracts.ValueObjects;

namespace WSC.CRM.Application.Mappings
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerResponseDto>();
            CreateMap<CreateCustomerDto, Customer>()
                .ForMember(dest => dest.CxAddress, opt => opt.MapFrom(src => new Address
                {
                    Street = src.Street,
                    City = src.City,
                    State = src.State,
                    ZipCode = src.ZipCode,
                    Country = src.Country
                }));
            CreateMap<UpdateCustomerDto, Customer>()
                 .ForMember(dest => dest.CxAddress, opt => opt.MapFrom(src => new Address
                 {
                     Street = src.Street,
                     City = src.City,
                     State = src.State,
                     ZipCode = src.ZipCode,
                     Country = src.Country
                 })); ;
        }
    }
}
