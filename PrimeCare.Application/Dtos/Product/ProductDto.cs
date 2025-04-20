namespace PrimeCare.Application.Dtos.Product;

/// <summary>
/// Data Transfer Object for a product.
/// </summary>
public class ProductDto : BaseProductDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the type of the category.
    /// </summary>
    public string Category { get; set; } = null!;

    /// <summary>
    /// Gets or sets the brand of the product.
    /// </summary>
    public string ProductBrand { get; set; } = null!;

}