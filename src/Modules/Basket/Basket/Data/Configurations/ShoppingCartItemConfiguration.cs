using Basket.Basket.Modules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Basket.Data.Configurations
{
    internal class ShoppingCartItemConfiguration : IEntityTypeConfiguration<ShoppingCartItem>
    {
        public void Configure(EntityTypeBuilder<ShoppingCartItem> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.ProductId).IsRequired();
            builder.Property(p => p.Quantity).IsRequired();
            builder.Property(p => p.Color);
            builder.Property(p => p.Price).IsRequired();
            builder.Property(p => p.ProductName).IsRequired();
        }
    }
}
