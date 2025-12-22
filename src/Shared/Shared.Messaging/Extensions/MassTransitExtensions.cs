using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Shared.Messaging.Extensions
{
    public static class MassTransitExtensions
    {
        public static IServiceCollection AddMassTransitWithAssemblies(this IServiceCollection services, IConfiguration configuration, params Assembly[] assemblies)
        {
            // Register MassTransit and scan provided assemblies for consumers, sagas, state machines and activities.
            services.AddMassTransit(config =>
            {
                // Use kebab-case for generated endpoint names (e.g. "order-submitted" instead of "OrderSubmitted").
                config.SetKebabCaseEndpointNameFormatter();

                // Configure an in-memory saga repository provider.
                // Useful for development and tests; replace with a durable provider (e.g. EF, Mongo) in production.
                config.SetInMemorySagaRepositoryProvider();

                // Register message consumers found in the provided assemblies.
                config.AddConsumers(assemblies);

                // Register saga state machines found in the provided assemblies.
                config.AddSagaStateMachines(assemblies);

                // Register saga definitions (state instances) found in the provided assemblies.
                config.AddSagas(assemblies);

                // Register activity contracts (for routing slip activities) found in the provided assemblies.
                config.AddActivities(assemblies);


                config.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(new Uri(configuration["MessageBroker:Host"]!), host =>
                    {
                        host.Username(configuration["MessageBroker:UserName"]!);
                        host.Password(configuration["MessageBroker:Password"]!);
                    });
                    configurator.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
