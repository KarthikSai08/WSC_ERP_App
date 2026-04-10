using Microsoft.Extensions.DependencyInjection;
using WSC.CRM.Application.Interfaces;
using WSC.CRM.Application.Interfaces.Repository;
using WSC.CRM.Infrastructure.Persistence.Context;
using WSC.CRM.Infrastructure.Repositories;

namespace WSC.CRM.Infrastructure.DependencyInjection
{
    public static class InfrastructureServices
    {
        public static IServiceCollection AddCRMInfrastructureService(this IServiceCollection service)
        {

            service.AddScoped<DapperContext>();

            service.AddScoped<ILeadRepository, LeadRepository>();
            service.AddScoped<ICustomerRepository, CustomerRepository>();
            service.AddScoped<IActivityRepository, ActivityRepository>();
            service.AddScoped<IOpportunityRepository, OpportunityRepository>();
            service.AddScoped<IRedisCacheService, RedisCacheService>();
            return service;
        }
    }
}
