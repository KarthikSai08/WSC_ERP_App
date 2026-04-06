using FluentValidation;
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

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            /*services.AddValidatorsFromAssemblyContaining<CreateActivityValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateCustomerValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateLeadValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateOpportunityValidator>();

            services.AddValidatorsFromAssemblyContaining<UpdateCustomerValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateLeadValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateOpportunityValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateActivityValidator>();*/

            return services;
        }
    }
}