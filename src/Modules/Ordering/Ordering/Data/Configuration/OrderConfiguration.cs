using Microsoft.EntityFrameworkCore;
using Ordering.Models;

namespace Ordering.Data.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            // Properties
            builder.Property(o => o.CustomerId);

            builder.HasIndex(o => o.OrderName).IsUnique();


            builder.Property(o => o.OrderName).IsRequired().HasMaxLength(100);

            // Computed or convenience properties   
            builder.HasMany(o => o.Items)
                .WithOne()
                .HasForeignKey(si => si.OrderId);

            builder.ComplexProperty(
                o => o.ShippingAddress, addressBuilder =>
                {
                    addressBuilder.Property(a => a.FirstName).HasMaxLength(100).IsRequired();

                    addressBuilder.Property(a => a.LastName).HasMaxLength(50).IsRequired();
                    addressBuilder.Property(a => a.EmailAddress).HasMaxLength(50);

                    addressBuilder.Property(a => a.AddressLine).HasMaxLength(180).IsRequired();

                    addressBuilder.Property(a => a.Country).HasMaxLength(50);

                    addressBuilder.Property(a => a.State).HasMaxLength(50);

                    addressBuilder.Property(a => a.ZipCode).HasMaxLength(5).IsRequired();
                });

            builder.ComplexProperty(
              o => o.Payment, addressBuilder =>
              {
                  addressBuilder.Property(a => a.CardName).HasMaxLength(50).IsRequired();

                  addressBuilder.Property(a => a.CardNumber).HasMaxLength(24).IsRequired();

                  addressBuilder.Property(a => a.Expiration).HasMaxLength(10);

                  addressBuilder.Property(a => a.CVV).HasMaxLength(3);

                  addressBuilder.Property(a => a.PaymentMethod);
              });
        }
    }
}
