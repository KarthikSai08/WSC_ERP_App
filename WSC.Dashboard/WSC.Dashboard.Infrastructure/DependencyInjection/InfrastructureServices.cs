using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using WSC.Dashboard.Application.Interfaces.RepositoryInterfaces;
using WSC.Dashboard.Infrastructure.Persistence.Context;
using WSC.Dashboard.Infrastructure.Repositories;

namespace WSC.Dashboard.Infrastructure.DependencyInjection
{
    public static class InfrastructureServices
    {
        public static IServiceCollection AddDashboardInfrastuctureService(this IServiceCollection services)
        {
            services.AddScoped<DapperContext>();
                
            services.AddScoped<IDashboardRepository, DashboardRepository>();

            return services;
        }
    }
}
