namespace PrimeCare.Shared.Dtos.ProductBrand;

/// <summary>
/// Data Transfer Object for a product brand.
/// </summary>
public class ProductBrandDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the product brand.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the product brand.
    /// </summary>
    public string Name { get; set; } = null!;
}