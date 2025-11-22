using Basket.Basket.Modules;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Basket.Data
{
    public class BasketDbContext : DbContext
    {
        public BasketDbContext(DbContextOptions<BasketDbContext> options) : base(options) { }

        public DbSet<ShoppingCart> ShoppingCarts => Set<ShoppingCart>();
        public DbSet<ShoppingCartItem> ShoppingCartItems => Set<ShoppingCartItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShoppingCart>()
                .Property(c => c.UserName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.HasDefaultSchema("basket");
            //Find every class in this assembly that implements IEntityTypeConfiguration<T> and apply its configuration automatically.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
