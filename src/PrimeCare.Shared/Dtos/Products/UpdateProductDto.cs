using PrimeCare.Shared.Dtos.Photos;

namespace PrimeCare.Shared.Dtos.Products;

/// <summary>
/// Data Transfer Object for updating a product.
/// </summary>
public class UpdateProductDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the product.
    /// </summary>
    public int Id { get; set; }

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
    /// Gets or sets the collection of photos associated with this product.
    /// Initialized as an empty list to avoid null reference exceptions.
    /// </summary>
    public ICollection<ProductPhotoDto> ProductPhotos { get; set; } = null!;

    /// <summary>
    /// Gets or sets the main image URL for the product.
    /// </summary>
    public string PhotoUrl { get; set; } = null!;
}
