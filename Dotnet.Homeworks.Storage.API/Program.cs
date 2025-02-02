using Dotnet.Homeworks.Storage.API.Configuration;
using Dotnet.Homeworks.Storage.API.Endpoints;
using Dotnet.Homeworks.Storage.API.Services;
using Dotnet.Homeworks.Storage.API.ServicesExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IStorageFactory, StorageFactory>();
builder.Services.AddHostedService<PendingObjectsProcessor>();

builder.Services.Configure<MinioConfig>(builder.Configuration.GetSection("MinioConfig"));
var minioConfig = builder.Configuration.GetSection("MinioConfig").Get<MinioConfig>()!;
builder.Services.AddMinioClient(minioConfig);

var app = builder.Build();

app.MapProductsEndpoints();

app.Run();