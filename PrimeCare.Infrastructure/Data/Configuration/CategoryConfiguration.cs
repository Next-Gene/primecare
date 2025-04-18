
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrimeCare.Core.Entities;

namespace PrimeCare.Infrastructure.Data.Configuration;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
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
            .HasMaxLength(300)
            .IsRequired();

        builder
             .Property(p => p.Slug)
             .HasColumnType("varchar")
             .HasMaxLength(150)
             .IsRequired();
        builder
            .Property(e => e.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");


        builder
            .Property(e => e.UpdatedAt)
            .HasDefaultValueSql("GETDATE()");



    }
}
