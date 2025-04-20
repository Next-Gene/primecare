namespace PrimeCare.Application.Dtos.Product;

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
    /// Gets or sets the URL of the product picture.
    /// </summary>
    public string PictureUrl { get; set; } = null!;

    public int Stock { get; set; }

}
