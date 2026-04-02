using Microsoft.Extensions.DependencyInjection;
using WSC.CRM.Application.Interfaces.Repository;
using WSC.CRM.Infrastructure.Persistence.Context;
using WSC.CRM.Infrastructure.Repositories;

namespace WSC.CRM.Infrastructure.DependencyInjection
{
    public static class InfrastructureServices
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection service)
        {

            service.AddScoped<DapperContext>();

            service.AddScoped<ILeadRepository, LeadRepository>();
            service.AddScoped<ICustomerRepository, CustomerRepository>();
            service.AddScoped<IActivityRepository, ActivityRepository>();

            return service;
        }
    }
}
