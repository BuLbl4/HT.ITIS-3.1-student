using Dotnet.Homeworks.Mailing.API.Configuration;
using Dotnet.Homeworks.Mailing.API.Consumers;
using MassTransit;

namespace Dotnet.Homeworks.Mailing.API.ServicesExtensions;

public static class AddMasstransitRabbitMqExtensions
{
    public static IServiceCollection AddMasstransitRabbitMq(this IServiceCollection services,
        RabbitMqConfig rabbitConfiguration)
    {
        services.AddMassTransit(busConf =>
        {
            busConf.SetKebabCaseEndpointNameFormatter();
            busConf.AddConsumer<EmailConsumer>();
            busConf.UsingRabbitMq((context, bus) =>
            {
                bus.Host($"rabbitmq://{rabbitConfiguration.Hostname}", conf =>
                {
                    conf.Username(rabbitConfiguration.Username);
                    conf.Password(rabbitConfiguration.Password);
                });

                bus.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}