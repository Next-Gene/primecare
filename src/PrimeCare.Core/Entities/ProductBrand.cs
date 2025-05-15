namespace PrimeCare.Core.Entities;

/// <summary>
/// Represents a brand of product.
/// </summary>
public class ProductBrand : BaseEntity
{
    /// <summary>
    /// Gets or sets the name of the product brand.
    /// </summary>
    public string Name { get; set; } = null!;
}
