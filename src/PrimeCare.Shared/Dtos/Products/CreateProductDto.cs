namespace PrimeCare.Shared.Dtos.Products;

/// <summary>
/// Data Transfer Object for creating a product.
/// </summary>
public class CreateProductDto
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
    /// Gets or sets the identifier of the category to which the product belongs.
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the brand of the product.
    /// </summary>
    public int ProductBrandId { get; set; }
}
