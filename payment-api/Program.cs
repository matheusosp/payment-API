using MediatR;
using Microsoft.EntityFrameworkCore;
using PaymentAPI.Application.Mapping;
using PaymentAPI.Domain.Interfaces;
using PaymentAPI.Infra.EF;
using PaymentAPI.Infra.EF.Context;
using PaymentAPI.Infra.EF.Repositories;
using System;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true);

if (bool.Parse(configuration.GetSection("UseInMemory").Value))
{
    services.AddDbContext<ApplicationDbContext>(
      (options) => options.UseInMemoryDatabase("PaymentAPI"));
}
else if (bool.Parse(configuration.GetSection("UseSqlite").Value))
{
    services.AddDbContext<ApplicationDbContext>(cfg =>
    {
        cfg.UseSqlite("Data Source=Database\\Sales.db");
    });
}
else
{
    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));
}

services.AddControllers();
services.AddScoped<IUnitOfWork, UnitOfWork>();
services.AddScoped(typeof(ISaleRepository), typeof(SaleRepository));
services.AddMediatR(AppDomain.CurrentDomain.Load("PaymentAPI.Application"));
services.AddAutoMapper(typeof(AutoMapperInitializer));

services.AddEndpointsApiExplorer();
services.AddSwaggerGen(cfg => {
    cfg.IncludeXmlComments(string.Format(@"{0}\PaymentAPI.xml", AppDomain.CurrentDomain.BaseDirectory));
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "PaymentAPI");
    options.RoutePrefix = "api-docs";
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
