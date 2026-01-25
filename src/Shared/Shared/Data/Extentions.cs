using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data.Seed;

namespace Shared.Data
{
    /// <summary>
    /// Provides extension methods related to database migrations for <see cref="IApplicationBuilder"/>.
    /// </summary>
    public static class Extentions
    {
        /// <summary>
        /// Applies any pending Entity Framework Core migrations for the specified <typeparamref name="TContext"/>
        /// when the application starts.
        /// </summary>
        /// <typeparam name="TContext">The <see cref="DbContext"/> type for which to apply migrations.</typeparam>
        /// <param name="app">The application builder used to retrieve services and continue pipeline configuration.</param>
        /// <returns>The same <see cref="IApplicationBuilder"/> instance to allow fluent chaining.</returns>
        /// <remarks>
        /// This method calls an async helper and blocks on its completion using <c>GetAwaiter().GetResult()</c>,
        /// so it will block the calling thread until migrations have completed. Prefer calling during application
        /// startup before serving requests.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the <typeparamref name="TContext"/> is not registered in the application's service container.
        /// </exception>
        /// <exception cref="DbUpdateException">Propagates exceptions thrown by the migration operation.</exception>
        public static IApplicationBuilder UseMigration<TContext>(this IApplicationBuilder app) where TContext : DbContext
        {
            MigrateDatabaseAsync<TContext>(app.ApplicationServices).GetAwaiter().GetResult();

            SeedDataAsync(app.ApplicationServices).GetAwaiter().GetResult();

            return app;
        }

        /// <summary>
        /// Creates a service scope, resolves an instance of <typeparamref name="TContext"/>, and applies any pending migrations
        /// by calling <see cref="DatabaseFacade.MigrateAsync"/> on the context's <see cref="Database"/>.
        /// </summary>
        /// <typeparam name="TContext">The <see cref="DbContext"/> type to resolve and migrate.</typeparam>
        /// <param name="serviceProvider">The application's root <see cref="IServiceProvider"/> used to create a scoped provider.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous migration operation.</returns>
        /// <remarks>
        /// The method creates a scope to ensure a scoped <see cref="DbContext"/> is resolved and disposed correctly.
        /// Any exceptions thrown by service resolution or the underlying migration will be propagated to the caller.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// Thrown if an instance of <typeparamref name="TContext"/> cannot be resolved from the scoped service provider.
        /// </exception>
        /// <exception cref="DbUpdateException">Propagates exceptions thrown during migration execution.</exception>
        private static async Task MigrateDatabaseAsync<TContext>(IServiceProvider serviceProvider) where TContext : DbContext
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<TContext>();
            await context.Database.MigrateAsync();
        }

        private static async Task SeedDataAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var seeders = scope.ServiceProvider.GetServices<IDataSeeder>();
            foreach (var seed in seeders)
            {
                await seed.SeedAllAsync();
            }
        }
    }
}
