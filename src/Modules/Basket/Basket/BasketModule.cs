using Basket.Data;
using Basket.Data.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data;
using Shared.Data.Interceptors;

namespace Basket
{
    public static class BasketModule
    {
        public static IServiceCollection AddBasketModule(this IServiceCollection services, IConfiguration configuration)
        {


            services.AddScoped<IBasketRepository, BasketRepository>();
            services.Decorate<IBasketRepository, CachedBasketRepository>();

            // this is not recomended approach since it can be messy and cumbersome
            // it is better to use scrutor approach
            // we can do it like this if we want to avoid indecisivnes issues on IBasketRepository
            //services.AddScoped<IBasketRepository>(provider =>
            //{
            //    var basketRepository = provider.GetService<IBasketRepository>()!;
            //    return new CachedBasketRepository(basketRepository, provider.GetRequiredService<IDistributedCache>());
            //});

            var connectionString = configuration.GetConnectionString("Database");

            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

            services.AddDbContext<BasketDbContext>((serviceProvider, op) =>
            {
                op.AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>());
                op.UseNpgsql(connectionString);
            });
            return services;
        }

        public static IApplicationBuilder UseBasketModule(this IApplicationBuilder app)
        {

            // Apply database migrations for the CatalogDbContext to ensure the database schema is up-to-date.
            app.UseMigration<BasketDbContext>();
            return app;
        }
    }
}
