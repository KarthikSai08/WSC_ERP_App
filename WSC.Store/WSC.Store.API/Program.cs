using Microsoft.Data.SqlClient;
using Scalar.AspNetCore;
using System.Data;
using WSC.Shared.Contracts.Protos;
using WSC.Store.API.Filters;
using WSC.Store.API.Middleware;
using WSC.Store.API.Services;
using WSC.Store.Application.DependencyInjection;
using WSC.Store.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add Controllers with JSON options
builder.Services.AddControllers(opts =>
{
    opts.Filters.Add<ValidationFilter>();
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters
        .Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});

// Configure HTTP Client for REST API (ICustomerClient)
var crmUrl = builder.Configuration["Services:CRM"];
//builder.Services.AddHttpClient<ICustomerClient, CustomerClient>(client =>
//{
//    client.BaseAddress = new Uri(crmUrl ?? "https://localhost:7238");
//    client.Timeout = TimeSpan.FromSeconds(60);
//});

builder.Services.AddGrpcClient<CustomerService.CustomerServiceClient>(options =>
{
    options.Address = new Uri("http://localhost:5001");
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new SocketsHttpHandler
    {
        PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
        KeepAlivePingDelay = TimeSpan.FromSeconds(60),
        KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
        EnableMultipleHttp2Connections = true
    };
    return handler;
})
.ConfigureChannel(options =>
{
    options.HttpHandler = new SocketsHttpHandler
    {
        PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
        EnableMultipleHttp2Connections = true
    };
});
builder.Services.AddScoped<CustomerGrpcClientService>();
// Add OpenAPI/Swagger
builder.Services.AddOpenApi();

// Add Application Services
builder.Services.AddStoreApplicationService();
builder.Services.AddStoreInfrastructureService();

// Add Validation Filter
builder.Services.AddScoped<ValidationFilter>();

// Add Database Connection
builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(Program).Assembly);
});

// Add Logging
builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
});

var app = builder.Build();

// Use Exception Middleware
app.UseMiddleware<ExceptionMiddleware>();

// Use Development Middleware
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
