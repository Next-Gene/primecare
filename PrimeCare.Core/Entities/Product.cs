namespace PrimeCare.Core.Entities;

/// <summary>
/// Represents a product.
/// </summary>
public class Product : BaseEntity
{
    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the description of the product.
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// Gets or sets the price of the product.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the URL of the product picture.
    /// </summary>
    public string PictureUrl { get; set; } = null!;

    /// <summary>
    /// Gets or sets the type of the product.
    /// </summary>
    public ProductType ProductType { get; set; } = null!;

    /// <summary>
    /// Gets or sets the identifier of the product type.
    /// </summary>
    public int ProductTypeId { get; set; }

    /// <summary>
    /// Gets or sets the brand of the product.
    /// </summary>
    public ProductBrand ProductBrand { get; set; } = null!;

    /// <summary>
    /// Gets or sets the identifier of the product brand.
    /// </summary>
    public int ProductBrandId { get; set; }
}