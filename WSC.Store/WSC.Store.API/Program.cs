using Scalar.AspNetCore;
using WSC.Store.Application.DependencyInjection;
using WSC.Store.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
.AddJsonOptions(options =>
 {
     options.JsonSerializerOptions.Converters
         .Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
 });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddStoreApplicationService();
builder.Services.AddStoreInfrastructureService();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(Program).Assembly);
    cfg.AddMaps(typeof(ApplicationService).Assembly);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
