using PrimeCare.Shared.Dtos.Photos;

namespace PrimeCare.Shared.Dtos.Products;

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
    /// Gets or sets the collection of photos associated with this category.
    /// Initialized as an empty list to avoid null reference exceptions.
    /// </summary>
    public ICollection<ProductPhotosDto> ProductPhotos { get; set; } = null!;


    /// <summary>
    /// img url   
    /// </summary>
    public string PhotoUrl { get; set; } = null!;
}