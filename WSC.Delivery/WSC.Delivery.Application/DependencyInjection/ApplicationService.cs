using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace WSC.Delivery.Application.DependencyInjection
{
    public static class ApplicationService
    {
        public static IServiceCollection AddDeliveryApplicationService(this IServiceCollection services)
        {
            // Register validators from assembly
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
