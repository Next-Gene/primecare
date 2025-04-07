namespace PrimeCare.Application.Dtos.ProductType;

/// <summary>
/// Data Transfer Object for a product type.
/// </summary>
public class ProductTypeDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the product type.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the product type.
    /// </summary>
    public string Name { get; set; } = null!;
}