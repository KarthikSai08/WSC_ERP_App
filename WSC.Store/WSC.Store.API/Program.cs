using Microsoft.Data.SqlClient;
using Scalar.AspNetCore;
using System.Data;
using WSC.Shared.Contracts.Interfaces.CRMClients;
using WSC.Shared.Infrastructure.Clients;
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


builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

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
