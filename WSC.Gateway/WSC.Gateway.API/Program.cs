using Scalar.AspNetCore;
using Serilog;
using StackExchange.Redis;
using System.Text.Json.Serialization;
using WSC.Gateway.API.Filters;
using WSC.Gateway.API.Middleware;
using WSC.Gateway.Application.DependencyInjection;
using WSC.Gateway.Application.Interfaces.Clients;
using WSC.Gateway.Infrastructure.Clients;
using WSC.Gateway.Infrastructure.DependencyInjection;
using WSC.Gateway.Infrastructure.Http;

using WSC.Shared.Infrastructure.Extensions;
using WSC.Shared.Infrastructure.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(opts =>
{
    opts.Filters.Add<ValidationFilter>();
})
.AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.Converters
    .Add(new JsonStringEnumConverter());
});
builder.Services.AddScoped<ValidationFilter>();

builder.Services.AddOpenApi();
builder.Host.ConfigureSerilog();

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var config = builder.Configuration["Redis:ConnectionString"]
        ?? throw new InvalidOperationException("Redis:ConnectionString not configured");
    return ConnectionMultiplexer.Connect(config);
});

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddHttpContextAccessor();

builder.Services.AddGatewayApplicationServices();
builder.Services.AddGatewayInfrastructureService();

var crmUrl = builder.Configuration["Services:CRM"]
    ?? throw new InvalidOperationException("Services:CRM is not configured");
var storeUrl = builder.Configuration["Services:Store"]
    ?? throw new InvalidOperationException("Services:Store is not Configured");
var deliveryUrl = builder.Configuration["Services:Delivery"]
    ?? throw new InvalidOperationException("Services:Delivery is  not Configured");

builder.Services.AddHttpClient<ICrmAggregatorClient, CrmAggregatorClient>(client =>
{
    client.BaseAddress = new Uri(crmUrl);
    client.Timeout = TimeSpan.FromSeconds(10);
})
.AddHttpMessageHandler<BearerTokenHandler>();


builder.Services.AddHttpClient<IStoreAggregatorClient, StoreAggregatorClient>(client =>
{
    client.BaseAddress = new Uri(storeUrl);
    client.Timeout = TimeSpan.FromSeconds(10);
})
.AddHttpMessageHandler<BearerTokenHandler>();

builder.Services.AddHttpClient<IDeliveryAggregatorClient, DeliveryAggregatorClient>(client =>
{
    client.BaseAddress = new Uri(deliveryUrl);
    client.Timeout = TimeSpan.FromSeconds(10);
})
.AddHttpMessageHandler<BearerTokenHandler>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<CorrelationMiddleware>();
app.UseSerilogRequestLogging();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
