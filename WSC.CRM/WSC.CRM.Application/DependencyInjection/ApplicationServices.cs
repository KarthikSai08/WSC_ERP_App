using Microsoft.Extensions.DependencyInjection;

using WSC.CRM.Application.Interfaces.Services;
using WSC.CRM.Application.Services;
using AutoMapper;
using System.Reflection;
using WSC.CRM.Application.Mappings;

namespace WSC.CRM.Application.DependencyInjection
{
    public static class ApplicationServices
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.AddScoped<ICustomerService, CustomerService>();

            return services;
        }
    }
}