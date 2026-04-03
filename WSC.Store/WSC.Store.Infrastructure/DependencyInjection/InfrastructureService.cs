using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using WSC.Store.Application.Interfaces.RepositoryInterfaces;
using WSC.Store.Infrastructure.Repository;

namespace WSC.Store.Infrastructure.DependencyInjection
{
    public static class InfrastructureService
    {
        public static IServiceCollection AddStoreInfrastructureService(this IServiceCollection service)
        {
            // Register infrastructure services here (e.g., database contexts, repositories, etc.)
            // Example:
            service.AddScoped<IProductRepository, ProductRepository>();
            // service.AddScoped<IInventoryRepository, InventoryRepository>();
            // service.AddScoped<IOrderRepository, OrderRepository>();
            return service;
        }
    }
}
