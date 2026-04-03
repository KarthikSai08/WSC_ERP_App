using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace WSC.Store.Application.DependencyInjection
{
    public static class ApplicationService
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection service)
        {
            // Register application services here (e.g., business logic services, application layer services, etc.)
            // Example:
            // service.AddScoped<IOrderService, OrderService>();
            // service.AddScoped<IProductService, ProductService>();
            return service;
        }
    }
}
