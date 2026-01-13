using Microsoft.EntityFrameworkCore;
using Ordering.Models;
using System.Reflection;

namespace Ordering.Data
{
    public class OrderingDbContext : DbContext
    {
        public OrderingDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("ordering");
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
