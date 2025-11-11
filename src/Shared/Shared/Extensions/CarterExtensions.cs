using Carter;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Shared.Extensions
{
    public static class CarterExtensions
    {

        /// <summary>
        /// Registers Carter and adds all <see cref="ICarterModule"/> implementations found in the provided assemblies.
        /// Scans each assembly for types assignable to <see cref="ICarterModule"/> and supplies them to Carter's configuration.
        /// </summary>
        /// <param name="services">The service collection to add Carter modules to.</param>
        /// <param name="assemblies">One or more assemblies to scan for <see cref="ICarterModule"/> implementations.</param>
        /// <returns>The original <see cref="IServiceCollection"/> for chaining.</returns>
        public static IServiceCollection AddCarterWithAssemblies(this IServiceCollection services, params Assembly[] assemblies)
        {

            services.AddCarter(configurator: config =>
            {
                foreach (var assembly in assemblies)
                {
                    var modules = assembly.GetTypes()
                                  .Where(t => t.IsAssignableTo(typeof(ICarterModule))).ToArray();

                    config.WithModules(modules);
                }
            });

            return services;
        }
    }
}
