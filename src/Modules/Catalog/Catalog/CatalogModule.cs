using Catalog.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data;

namespace Catalog
{
    public static class CatalogModule
    {
        public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database");
            services.AddDbContext<CatalogDbContext>(op => op.UseNpgsql(connectionString));

            return services;
        }

        public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
        {

            // Use api services

            // use application use case services

            // use data service - infrastructure service
            app.UseMigration<CatalogDbContext>();
            return app;
        }

    }
}
