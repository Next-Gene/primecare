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
}