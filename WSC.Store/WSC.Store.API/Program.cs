using Scalar.AspNetCore;
using WSC.Store.API.Filters;
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


builder.Services.AddOpenApi();

builder.Services.AddStoreApplicationService();
builder.Services.AddStoreInfrastructureService();

builder.Services.AddScoped<ValidationFilter>();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(Program).Assembly);
    cfg.AddMaps(typeof(ApplicationService).Assembly);
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
