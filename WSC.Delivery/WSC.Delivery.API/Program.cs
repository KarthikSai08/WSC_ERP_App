using Scalar.AspNetCore;
using WSC.Delivery.Application.DependencyInjection;
using WSC.Delivery.Infrastructure.DependencyInjection;
using WSC.Shared.Contracts.Filters;

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


var app = builder.Build();

//app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
