using Microsoft.EntityFrameworkCore;
using PrimeCare.Core.Entities;
using System.Reflection;

namespace PrimeCare.Infrastructure.Data;

/// <summary>
/// Database context for PrimeCare application.
/// </summary>
public class PrimeCareContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PrimeCareContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
    public PrimeCareContext(DbContextOptions<PrimeCareContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the products.
    /// </summary>
    public DbSet<Product> Products { get; set; }

    /// <summary>
    /// Gets or sets the product brands.
    /// </summary>
    public DbSet<ProductBrand> ProductBrands { get; set; }

    /// <summary>
    /// Gets or sets the Categories.
    /// </summary>
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly
            .GetExecutingAssembly());
    }
}