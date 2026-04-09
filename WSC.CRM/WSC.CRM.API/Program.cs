using Microsoft.AspNetCore.Server.Kestrel.Core;
using Scalar.AspNetCore;
using WSC.CRM.API.Filters;
using WSC.CRM.API.Services;
using WSC.CRM.Application.DependencyInjection;
using WSC.CRM.Infrastructure.DependencyInjection;
using WSC.Store.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

//add gRPC
builder.Services.AddGrpc();
builder.Services.AddControllers(opts =>
{
    opts.Filters.Add<ValidationFilter>();
})
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters
            .Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5001, o => o.Protocols = HttpProtocols.Http2);
});

builder.Services.AddOpenApi();

builder.Services.AddCRMApplicationService();
builder.Services.AddCRMInfrastructureService();

//Filters
builder.Services.AddScoped<ValidationFilter>();

//AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(Program).Assembly);
    cfg.AddMaps(typeof(ApplicationServices).Assembly);
});

var app = builder.Build();

//map gRPC service
app.MapGrpcService<CustomerServiceImpl>();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapGet("/test", () => "WORKING");
app.MapGet("/", () => "gRPC CRM Running");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
