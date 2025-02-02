using Dotnet.Homeworks.Data.Extensions;
using Dotnet.Homeworks.DataAccess.Extensions;
using Dotnet.Homeworks.Infrastructure.Services;
using Dotnet.Homeworks.MainProject.Configuration;
using Dotnet.Homeworks.MainProject.ServicesExtensions.Infrastructure;
using Dotnet.Homeworks.MainProject.ServicesExtensions.Masstransit;
using Dotnet.Homeworks.MainProject.ServicesExtensions.Mediatr;
using Dotnet.Homeworks.Shared.Dto;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddScoped<ICommunicationService, CommunicationService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();

builder.Services.Configure<RabbitMqConfig>(builder.Configuration.GetSection(nameof(RabbitMqConfig)));
var rabbitMqConfig = builder.Configuration.GetSection(nameof(RabbitMqConfig)).Get<RabbitMqConfig>();
builder.Services.AddMasstransitRabbitMq(rabbitMqConfig!);
builder.Services.AddTransient<ResultFactory>();
builder.Services.AddDataLayer(builder.Configuration);

builder.Services.AddMediatR();
builder.Services.AddDataAccess(builder.Configuration);
builder.Services.AddInfrastructure();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello World!");

app.MapControllers();

app.Run();