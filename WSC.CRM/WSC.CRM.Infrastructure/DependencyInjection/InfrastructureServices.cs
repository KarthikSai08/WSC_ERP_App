using Microsoft.Extensions.DependencyInjection;
using WSC.CRM.Application.Interfaces;
using WSC.CRM.Application.Interfaces.Repository;
using WSC.CRM.Infrastructure.Persistence.Context;
using WSC.CRM.Infrastructure.Repositories;

namespace WSC.CRM.Infrastructure.DependencyInjection
{
    public static class InfrastructureServices
    {
        public static IServiceCollection AddCRMInfrastructureService(this IServiceCollection services)
        {

            services.AddScoped<DapperContext>();

            services.AddScoped<ILeadRepository, LeadRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IActivityRepository, ActivityRepository>();
            services.AddScoped<IOpportunityRepository, OpportunityRepository>();
            services.AddScoped<IRedisCacheService, RedisCacheService>();
            return services;
        }
    }
}
