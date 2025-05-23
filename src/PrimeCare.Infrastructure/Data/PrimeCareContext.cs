using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PrimeCare.Core.Entities;
using PrimeCare.Core.Entities.Order;
using PrimeCare.Core.Entities.OrderAggregate;

namespace PrimeCare.Infrastructure.Data;

/// <summary>
/// Database context for the PrimeCare application.
/// Manages entity sets and configuration for database access using Entity Framework Core.
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
    /// Gets or sets the products in the database.
    /// </summary>
    public DbSet<Product> Products { get; set; }

    /// <summary>
    /// Gets or sets the product brands in the database.
    /// </summary>
    public DbSet<ProductBrand> ProductBrands { get; set; }

    /// <summary>
    /// Gets or sets the categories in the database.
    /// </summary>
    public DbSet<Category> Categories { get; set; }

    /// <summary>
    /// Gets or sets the product photos in the database.
    /// </summary>
    public DbSet<ProductPhoto> ProductPhotos { get; set; }

    /// <summary>
    /// Gets or sets the category photos in the database.
    /// </summary>
    public DbSet<CategoryPhoto> CategoryPhotos { get; set; }


    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderItem> OrderItems { get; set; }

    public DbSet<DeliveryMethod> DeliveryMethods { get; set; }



    /// <summary>
    /// Configures the entity mappings and relationships for the context.
    /// Applies all configurations from the current assembly.
    /// </summary>
    /// <param name="modelBuilder">The builder used to construct the model for the context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
