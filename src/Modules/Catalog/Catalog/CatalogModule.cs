using Catalog.Data;
using Catalog.Seed;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data;
using Shared.Data.Seed;

namespace Catalog
{
    public static class CatalogModule
    {
        public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database");
            services.AddDbContext<CatalogDbContext>(op => op.UseNpgsql(connectionString));
            services.AddScoped<IDataSeeder, CatalogDataSeeder>();
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
