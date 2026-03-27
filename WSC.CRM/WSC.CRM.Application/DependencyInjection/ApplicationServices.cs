using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WSC.CRM.Application.Interfaces.Services;
using WSC.CRM.Application.Mappings;
using WSC.CRM.Application.Services;
using AutoMapper;

namespace WSC.CRM.Application.DependencyInjection
{
    public static class ApplicationServices
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddAutoMapper(typeof(CustomerProfile));

            services.AddScoped<ICustomerService, CustomerService>();

            return services;
        }
    }
}
