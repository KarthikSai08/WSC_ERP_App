using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WSC.Store.Application.Interfaces.ServiceInterfaces;
using WSC.Store.Application.Service;

namespace WSC.Store.Application.DependencyInjection
{
    public static class ApplicationService
    {
        public static IServiceCollection AddStoreApplicationService(this IServiceCollection service)
        {
            var assembly = Assembly.GetExecutingAssembly();
            service.AddScoped<IProductService, ProductService>();

            return service;
        }
    }
}
