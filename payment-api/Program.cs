using MediatR;
using Microsoft.EntityFrameworkCore;
using PaymentAPI.Application.Commands;
using PaymentAPI.Domain.Features;
using PaymentAPI.Domain.Interfaces;
using PaymentAPI.Infra.EF;
using PaymentAPI.Infra.EF.Context;
using PaymentAPI.Infra.EF.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));

builder.Services.AddControllers();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddMediatR(typeof(RegisterSaleCommand));
builder.Services.AddAutoMapper(typeof(RegisterSaleCommand));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
