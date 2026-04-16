using AutoMapper;
using WSC.Shared.Contracts.Dtos.StoreLayer;
using WSC.Store.Application.Dtos;
using WSC.Store.Domain.Entities;

namespace WSC.Store.Application.Mappings
{
    public class InventoryProfile : Profile
    {
        public InventoryProfile()
        {
            CreateMap<Inventory, InventoryResponseDto>();
            CreateMap<CreateInventoryRecordDto, Inventory>();
        }
    }
}
