namespace PrimeCare.Shared.Dtos.ProductBrand;

/// <summary>
/// Data Transfer Object for creating a product brand.
/// </summary>
public class CreateProductBrandDto
{
    /// <summary>
    /// Gets or sets the name of the product brand.
    /// </summary>
    public string Name { get; set; } = null!;
}
