using Microsoft.Extensions.DependencyInjection;
using WSC.Delivery.Application.Interfaces.RepositoryInterfaces;
using WSC.Delivery.Infrastructure.Persistence.Context;
using WSC.Delivery.Infrastructure.Repositories;

namespace WSC.Delivery.Infrastructure.DependencyInjection
{
    public static class InfrastructureService
    {
        public static IServiceCollection AddDeliveryInfrastructureService(this IServiceCollection services)
        {
            services.AddScoped<DapperContext>();

            services.AddScoped<IDeliveryRepository, DeliveryRepository>();
            services.AddScoped<IDeliveryAgentRepository, DeliveryAgentRepository>();
            services.AddScoped<IDeliveryItemRepository, DeliveryItemRepository>();
            services.AddScoped<IDeliveryTrackingRepository, DeliveryTrackingRepository>();

            return services;
        }
    }
}
