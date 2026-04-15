using Scalar.AspNetCore;
using Serilog;
using StackExchange.Redis;
using WSC.Delivery.Application.DependencyInjection;
using WSC.Delivery.Infrastructure.DependencyInjection;
using WSC.Shared.Contracts.Interfaces.CRMClients;
using WSC.Shared.Contracts.Interfaces.StoreClients;
using WSC.Shared.Infrastructure.Clients;
using WSC.Shared.Infrastructure.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
  .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters
            .Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddOpenApi();

// Add Delivery Services
builder.Services.AddDeliveryApplicationService();
builder.Services.AddDeliveryInfrastructureService();
//builder.Services.AddScoped<ValidationFilter>();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(Program).Assembly);
    cfg.AddMaps(typeof(ApplicationService).Assembly);
});
builder.Host.ConfigureSerilog();
var crmUrl = builder.Configuration["Services:CRM"];
var storeUrl = builder.Configuration["Services:Store"];
builder.Services.AddHttpClient<ICustomerClient, CustomerClient>(client =>
{
    client.BaseAddress = new Uri(crmUrl);
    client.Timeout = TimeSpan.FromSeconds(60);
});
builder.Services.AddHttpClient<IProductClient, ProductClient>(client =>
{
    client.BaseAddress = new Uri(storeUrl);
    client.Timeout = TimeSpan.FromSeconds(60);
});
builder.Services.AddHttpClient<IOrderClient, OrderClient>(client =>
{
    client.BaseAddress = new Uri(storeUrl);
    client.Timeout = TimeSpan.FromSeconds(60);
});

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var config = builder.Configuration["Redis:ConnectionString"];
    return ConnectionMultiplexer.Connect(config);
});

var app = builder.Build();

//app.UseMiddleware<ExceptionMiddleware>();
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
