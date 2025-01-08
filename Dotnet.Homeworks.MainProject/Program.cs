using Dotnet.Homeworks.Data.Extensions;
using Dotnet.Homeworks.MainProject.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddDataLayer(builder.Configuration);

builder.Services.AddSingleton<IRegistrationService, RegistrationService>();
builder.Services.AddSingleton<ICommunicationService, CommunicationService>();

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