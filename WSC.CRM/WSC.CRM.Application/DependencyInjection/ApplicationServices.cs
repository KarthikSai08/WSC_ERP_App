using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WSC.CRM.Application.Interfaces.Services;
using WSC.CRM.Application.Services;

namespace WSC.CRM.Application.DependencyInjection
{
    public static class ApplicationServices
    {
        public static IServiceCollection AddCRMApplicationService(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ILeadService, LeadService>();
            services.AddScoped<IActivityService, ActivityService>();
            services.AddScoped<IOpportunityService, OpportunityService>();

            return services;
        }
    }
}