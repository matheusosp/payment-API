using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.PlatformAbstractions;
using PaymentAPI.Application.Commands;
using PaymentAPI.Application.Mapping;
using PaymentAPI.Application.Queries;
using PaymentAPI.Domain.Features;
using PaymentAPI.Domain.Interfaces;
using PaymentAPI.Infra.EF;
using PaymentAPI.Infra.EF.Context;
using PaymentAPI.Infra.EF.Repositories;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
if (bool.Parse(builder.Configuration["UseInMemory"]))
{
    builder.Services.AddDbContext<ApplicationDbContext>(
      (options) => options.UseInMemoryDatabase("PaymentAPI"));
}
else 
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));
}


builder.Services.AddControllers();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddMediatR(typeof(RegisterSaleCommandHandler));
builder.Services.AddMediatR(typeof(UpdateSaleCommandHandler));
builder.Services.AddMediatR(typeof(RetrieveSaleByIdQueryHandler));
builder.Services.AddAutoMapper(typeof(AutoMapperInitializer));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(cfg => {
    var pathProject = PlatformServices.Default.Application.ApplicationBasePath; ;
    var nameProject = $"{PlatformServices.Default.Application.ApplicationName}.xml";
    var PathXmlFile = Path.Combine(pathProject, nameProject);

    cfg.IncludeXmlComments(PathXmlFile);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "PaymentAPI");
        options.RoutePrefix = "api-docs";
    });

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
