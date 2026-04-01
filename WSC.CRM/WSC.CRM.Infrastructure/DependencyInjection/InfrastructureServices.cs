using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using WSC.CRM.Application.Interfaces;
using WSC.CRM.Infrastructure.Persistence.Context;
using WSC.CRM.Infrastructure.Repositories;

namespace WSC.CRM.Infrastructure.DependencyInjection
{
    public static  class InfrastructureServices
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection service)
        {

            service.AddScoped<DapperContext>();

            service.AddScoped<ICustomerRepository, CustomerRepository>();            
            
            return service;
        }
    }
}
