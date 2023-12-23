using CatsApp.Api.Filters;
using CatsApp.Api.Mapping;
using CatsApp.Api.Validators;
using CatsApp.Application.Queries.Handlers;
using CatsApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using CatsApp.Domain.Persistence;
using EasyNetQ;
using FluentValidation;
using FluentValidation.AspNetCore;
using Npgsql;
using System.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole();
using var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder
    .SetMinimumLevel(LogLevel.Trace)
    .AddConsole());

ILogger<ExceptionHandlingFilter> logger = loggerFactory.CreateLogger<ExceptionHandlingFilter>();

//builder.Services.AddDbContext<ICatContext, EFCatContext>(options => 
//options.UseSqlServer(builder.Configuration.GetConnectionString("MsSqlConnection")));
builder.Services.AddScoped<IDbConnection>((sp) => new NpgsqlConnection(builder.Configuration.GetConnectionString("PostgeSQLConnection")));
builder.Services.AddScoped<ICatContext, DapperCatContext>();

builder.Services.AddControllers(options => options.Filters.Add(new ExceptionHandlingFilter(logger)));
//.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CatValidator>());
builder.Services.AddValidatorsFromAssemblyContaining<CatValidator>();
SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions.ServiceCollectionExtensions.AddFluentValidationAutoValidation(builder.Services);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(CatMappingProfile));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(GetCarQueryHandler))));
builder.Services.AddSingleton(sp => RabbitHutch.CreateBus("host=localhost"));
builder.Services.AddScoped<IMessageProducer, MqRabbitMessageProducer>();

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
