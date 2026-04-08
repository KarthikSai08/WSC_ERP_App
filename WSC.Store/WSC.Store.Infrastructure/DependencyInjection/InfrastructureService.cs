using Microsoft.Extensions.DependencyInjection;
using WSC.CRM.Application.Interfaces.Repository;
using WSC.CRM.Infrastructure.Repositories;
using WSC.Store.Application.Interfaces.RepositoryInterfaces;
using WSC.Store.Infrastructure.Persistence.Context;
using WSC.Store.Infrastructure.Repository;

namespace WSC.Store.Infrastructure.DependencyInjection
{
    public static class InfrastructureService
    {
        public static IServiceCollection AddStoreInfrastructureService(this IServiceCollection service)
        {
            service.AddScoped<DapperContext>();

            service.AddScoped<IProductRepository, ProductRepository>();
            service.AddScoped<IInventoryRepository, InventoryRepository>();
            service.AddScoped<IOrderRepository, OrderRepository>();
            service.AddScoped<IOrderItemsRepository, OrderItemsRepository>();

            return service;
        }
    }
}
