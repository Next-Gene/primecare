using Microsoft.EntityFrameworkCore;
using PrimeCare.Core.Entities.Order;

namespace PrimeCare.Infrastructure.Data.Configuration
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<OrderItem> builder)
        {
            builder.OwnsOne(o => o.ItemOrderd, a =>
            {
                a.WithOwner();
            });
            builder.Property(s => s.Price).HasColumnType("decimal(18,2)");
        }
    }

}

