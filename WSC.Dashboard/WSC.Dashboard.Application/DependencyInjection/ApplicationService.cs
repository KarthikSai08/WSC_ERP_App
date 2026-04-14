using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using WSC.Dashboard.Application.Interfaces.ServiceInterfaces;
using WSC.Dashboard.Application.Services;

namespace WSC.Dashboard.Application.DependencyInjection
{
    public static class ApplicationService
    {
        public static IServiceCollection AddDashboardApplicationService(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.AddScoped<IDashboardService, DashboardService>();
            return services;
        }
    }
}
