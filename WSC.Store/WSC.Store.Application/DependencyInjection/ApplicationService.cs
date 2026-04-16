using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WSC.Store.Application.Interfaces.ServiceInterfaces;
using WSC.Store.Application.Service;

namespace WSC.Store.Application.DependencyInjection
{
    public static class ApplicationService
    {
        public static IServiceCollection AddStoreApplicationService(this IServiceCollection service)
        {
            var assembly = Assembly.GetExecutingAssembly();
            service.AddScoped<IProductService, ProductService>();
            service.AddScoped<IOrderService, OrderService>();
            service.AddScoped<IInventoryService, InventoryService>();
            service.AddScoped<IOrderItemsService, OrderItemService>();

            service.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return service;
        }
    }
}
