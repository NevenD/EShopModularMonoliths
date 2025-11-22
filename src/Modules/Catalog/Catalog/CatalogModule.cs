using Catalog.Data;
using Catalog.Seed;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Behaviors;
using Shared.Data;
using Shared.Data.Interceptors;
using Shared.Data.Seed;

namespace Catalog
{
    public static class CatalogModule
    {
        /// <summary>
        /// Register Catalog module services into the application's DI container.
        /// This includes MediatR handlers, EF Core DbContext with interceptors, and the data seeder.
        /// </summary>
        /// <param name="services">The IServiceCollection to register services into.</param>
        /// <param name="configuration">Application configuration used to read connection strings.</param>
        /// <returns>The same IServiceCollection for chaining.</returns>
        public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
        {
            // Register MediatR and scan the current assembly for handlers, requests, and notifications.
            services.AddMediatR(config =>
            {
                // Use reflection to register handlers from this assembly.
                config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());




                //typeof(ValidationBehavior<,>) refers to that open generic definition.
                //MediatR closes it (e.g., ValidationBehavior<CreateUserCommand, UserDto>) only when the request type satisfies the constraint where TRequest : ICommand<TResponse>.
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
                config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            });
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            var connectionString = configuration.GetConnectionString("Database");


            // Both are registered as scoped so they can capture scoped services (like current user, mediator, etc.).
            // The interceptors are created by the DI container
            // this means their constructors can receive any other registered service in our example Meditor in DispatchDomainEventsInterceptor
            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

            // Configure the CatalogDbContext:
            // - Resolve all registered ISaveChangesInterceptor instances from DI and add them to EF Core.
            // - Configure Npgsql as the database provider with the obtained connection string.
            services.AddDbContext<CatalogDbContext>((serviceProvider, op) =>
            {
                // Add any registered interceptors (auditing, domain events) to the DbContext pipeline.
                op.AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>());

                op.UseNpgsql(connectionString);
            });

            services.AddScoped<IDataSeeder, CatalogDataSeeder>();

            return services;
        }

        /// <summary>
        /// Configure and initialize the Catalog module at application startup.
        /// Applies EF Core migrations for the CatalogDbContext.
        /// </summary>
        /// <param name="app">The IApplicationBuilder used to configure the HTTP request pipeline.</param>
        /// <returns>The same IApplicationBuilder for chaining.</returns>
        public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
        {


            // Apply database migrations for the CatalogDbContext to ensure the database schema is up-to-date.
            app.UseMigration<CatalogDbContext>();

            return app;
        }
    }
}
