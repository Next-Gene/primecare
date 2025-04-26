using PrimeCare.Shared.Dtos.Photos;

namespace PrimeCare.Shared.Dtos.Products;

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
    /// Gets or sets the collection of photos associated with this category.
    /// Initialized as an empty list to avoid null reference exceptions.
    /// </summary>
    public ICollection<ProductPhotosDto> ProductPhotos { get; set; } = null!;

    public string PhotoUrl { get; set; } = null!;

}
