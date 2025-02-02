using Dotnet.Homeworks.Mediator.DependencyInjectionExtensions;

namespace Dotnet.Homeworks.MainProject.ServicesExtensions.Mediatr;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        services.AddMediator();
        return services;
    }
}