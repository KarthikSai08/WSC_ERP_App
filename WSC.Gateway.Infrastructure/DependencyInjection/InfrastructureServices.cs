using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using WSC.Gateway.Application.Interfaces;
using WSC.Gateway.Infrastructure.Persistence;
using WSC.Gateway.Infrastructure.Repositories;

namespace WSC.Gateway.Infrastructure.DependencyInjection
{
    public static class InfrastructureServices
    {
        public static IServiceCollection AddGatewayInfrastructureService(this IServiceCollection services)
        {
            services.AddScoped<DapperContext>();

            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            return services;
        }
    }
}
