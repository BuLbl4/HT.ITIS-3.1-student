using Dotnet.Homeworks.Data.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnet.Homeworks.Data.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddDataLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Default"),
                builder => builder.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));
    }

}