using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Data.SqlClient;
using Scalar.AspNetCore;
using Serilog;
using StackExchange.Redis;
using System.Data;
using WSC.Shared.Contracts.Interfaces.CRMClients;
using WSC.Shared.Infrastructure.Clients;
using WSC.Shared.Infrastructure.Logging;
using WSC.Store.API.Filters;
using WSC.Store.API.Middleware;
using WSC.Store.API.RateLimiting;
using WSC.Store.Application.DependencyInjection;
using WSC.Store.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(opts =>
{
    opts.Filters.Add<ValidationFilter>();
})
.AddJsonOptions(options =>
 {
     options.JsonSerializerOptions.Converters
         .Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
 });

var crmUrl = builder.Configuration["Services:CRM"];

builder.Services.AddHttpClient<ICustomerClient, CustomerClient>(client =>
{
    client.BaseAddress = new Uri(crmUrl);
    client.Timeout = TimeSpan.FromSeconds(60);
});
builder.Services.AddOpenApi();

builder.Services.AddStoreApplicationService();
builder.Services.AddStoreInfrastructureService();

builder.Services.AddScoped<ValidationFilter>();
builder.Host.ConfigureSerilog();

builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var config = builder.Configuration["Redis:ConnectionString"];
    return ConnectionMultiplexer.Connect(config);
});

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(Program).Assembly);
    cfg.AddMaps(typeof(ApplicationService).Assembly);
});

builder.Services.AddCustomRateLimiting();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<CorrelationMiddleware>();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
