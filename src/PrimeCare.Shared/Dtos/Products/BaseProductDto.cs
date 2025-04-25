namespace PrimeCare.Shared.Dtos.Products;

public class BaseProductDto
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
    /// Collection of photos associated with this product.
    /// </summary>
    public string PictureUrl { get; set; } = null!;
}
