using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrimeCare.Core.Entities;

namespace PrimeCare.Infrastructure.Data.Configuration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder
            .Property(p => p.Id)
            .IsRequired();
        builder
            .Property(p => p.Name)
            .HasColumnType("varchar")
            .HasMaxLength(100)
            .IsRequired();
        builder
            .Property(p => p.Description)
            .HasColumnType("varchar")
            .HasMaxLength(180)
            .IsRequired();
        builder
            .Property(p => p.Price)
            .HasColumnType("decimal(18,2)");
        builder
            .HasOne(b => b.ProductBrand)
            .WithMany()
            .HasForeignKey(p => p.ProductBrandId);
        builder
            .HasOne(t => t.ProductType)
            .WithMany()
            .HasForeignKey(p => p.ProductTypeId);

    }
}
