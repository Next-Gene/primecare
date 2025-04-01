using Microsoft.EntityFrameworkCore;
using PrimeCare.Core.Entities;
using System.Reflection;

namespace PrimeCare.Infrastructure.Data;

public class PrimeCareContext : DbContext
{
    public PrimeCareContext(DbContextOptions<PrimeCareContext> options)
        : base(options)
    {

    }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductBrand> ProductBrands { get; set; }
    public DbSet<ProductType> ProductTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly
            .GetExecutingAssembly());
    }
}
