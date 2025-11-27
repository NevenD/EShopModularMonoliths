using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shared.Behaviors;
using System.Reflection;

namespace Shared.Extensions
{
    public static class MediatrExtensions
    {
        public static IServiceCollection AddMediatrWithAssemblies(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblies(assemblies);
                //typeof(ValidationBehavior<,>) refers to that open generic definition.
                //MediatR closes it (e.g., ValidationBehavior<CreateUserCommand, UserDto>) only when the request type satisfies the constraint where TRequest : ICommand<TResponse>.
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
                config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            });

            services.AddValidatorsFromAssemblies(assemblies);
            return services;
        }
    }
}
