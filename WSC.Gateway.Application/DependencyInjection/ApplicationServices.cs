using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WSC.Gateway.Application.Interfaces;
using WSC.Gateway.Application.Services;
using FluentValidation;

namespace WSC.Gateway.Application.DependencyInjection
{
    public static class ApplicationServices
    {
        public static IServiceCollection AddGatewayApplicationServices(this IServiceCollection services)
        {

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAggregatorService, AggregatorService>();

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
