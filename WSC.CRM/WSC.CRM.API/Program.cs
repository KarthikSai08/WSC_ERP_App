using Scalar.AspNetCore;
using Serilog;
using StackExchange.Redis;
using WSC.CRM.API.Filters;
using WSC.CRM.API.Middleware;
using WSC.CRM.Application.DependencyInjection;
using WSC.CRM.Infrastructure.DependencyInjection;
using WSC.Shared.Infrastructure.Logging;
using WSC.Shared.Infrastructure.Services;
using WSC.Store.API.Middleware;

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
builder.Services.AddOpenApi();

builder.Services.AddScoped<IdempotencyService>();
//Dependency Injection for Application and Infrastructure layers
builder.Services.AddCRMApplicationService();
builder.Services.AddCRMInfrastructureService();

//Filter
builder.Services.AddScoped<ValidationFilter>();
//Logging
builder.Host.ConfigureSerilog();

//Redis Configuration
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var config = builder.Configuration["Redis:ConnectionString"];
    return ConnectionMultiplexer.Connect(config);
});

//AutoMapper Configuration
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(Program).Assembly);
    cfg.AddMaps(typeof(ApplicationServices).Assembly);
});

var app = builder.Build();

//Custom Middleware for Exception Handling and Correlation ID
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<IdempotencyMiddleware>();
app.UseMiddleware<CorrelationMiddleware>();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

//app.MapGet("/test", () => "WORKING");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
