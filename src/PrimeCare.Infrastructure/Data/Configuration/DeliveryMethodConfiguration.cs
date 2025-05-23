using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrimeCare.Core.Entities.Order;

namespace PrimeCare.Infrastructure.Data.Configuration
{
    internal class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(s => s.Price).HasColumnType("decimal(18,2)");
        }
    }
}
