using Microsoft.Extensions.Hosting;
using Serilog;

namespace WSC.Shared.Infrastructure.Logging
{
    public static class SerilogExtension
    {
        public static void ConfigureSerilog(this IHostBuilder host)
        {
            host.UseSerilog((ctx, services, config) =>
            {
                config.ReadFrom.Configuration(ctx.Configuration)
                                .ReadFrom.Services(services)
                                .Enrich.FromLogContext();
            });
        }
    }
}
