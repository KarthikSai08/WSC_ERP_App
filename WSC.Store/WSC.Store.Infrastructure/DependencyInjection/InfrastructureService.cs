using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace WSC.Store.Infrastructure.DependencyInjection
{
    public static class InfrastructureService
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection service)
        {
            // Register infrastructure services here (e.g., database contexts, repositories, etc.)
            // Example:
            // service.AddScoped<IProductRepository, ProductRepository>();
            // service.AddScoped<IInventoryRepository, InventoryRepository>();
            // service.AddScoped<IOrderRepository, OrderRepository>();
            return service;
        }
    }
}
