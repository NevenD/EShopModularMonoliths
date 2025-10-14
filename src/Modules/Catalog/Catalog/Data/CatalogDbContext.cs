

namespace Catalog.Data
{
    public class CatalogDbContext : DbContext
    {
        // Constructor that accepts DbContextOptions to configure the context (e.g., connection string, provider).
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
        {
        }

        // DbSet representing the Products table in the database.
        // Allows querying and saving instances of Product.
        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("catalog");
            // Applies all IEntityTypeConfiguration implementations from the current assembly to the model builder.
            // This enables automatic configuration of entity mappings and constraints defined in separate configuration classes.
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }
}
