using System.Reflection;
using GenericRepository.Settings;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RabbitMqExtension;

public static class Extensions
{
    public static IServiceCollection AddMassTransitRabbitMq(this IServiceCollection services)
    {
        services.AddMassTransit(config => 
        {
            config.AddConsumers(Assembly.GetEntryAssembly());

            config.UsingRabbitMq((context, configurator) => 
            {
                var configuration = context.GetService<IConfiguration>() 
                                    ?? throw new Exception("IConfiguration object is null");
                var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>() 
                                      ?? throw new Exception("Service settings object is null");
                var rabbitMqSettings = configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>() 
                                       ?? throw new Exception("Rabbit MQ settings object is null");
                configurator.Host(rabbitMqSettings.Host);
                configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));
            });
        });
        
        return services;
    }
}