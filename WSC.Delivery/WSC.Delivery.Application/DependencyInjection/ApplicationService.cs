using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WSC.Delivery.Application.Interfaces.ServiceInterfaces;
using WSC.Delivery.Application.Services;

namespace WSC.Delivery.Application.DependencyInjection
{
    public static class ApplicationService
    {
        public static IServiceCollection AddDeliveryApplicationService(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddScoped<IDeliveryAgentService, DeliveryAgentService>();
            services.AddScoped<IDeliveryItemService, DeliveryItemService>();
            services.AddScoped<IOrderDeliveryService, OrderDeliveryService>();
            services.AddScoped<IDeliveryTrackingService, DeliveryTrackingService>();

            services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));

            return services;
        }
    }
}
