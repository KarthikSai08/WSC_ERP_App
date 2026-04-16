using AutoMapper;
using WSC.Shared.Contracts.Dtos.StoreLayer;
using WSC.Store.Application.Dtos;
using WSC.Store.Domain.Entities;

namespace WSC.Store.Application.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductResponseDto>();
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();
        }
    }
}
