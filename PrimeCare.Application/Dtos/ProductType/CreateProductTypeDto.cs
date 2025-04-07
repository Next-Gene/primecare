namespace PrimeCare.Application.Dtos.ProductType;

/// <summary>
/// Data Transfer Object for creating a product type.
/// </summary>
public class CreateProductTypeDto
{
    /// <summary>
    /// Gets or sets the name of the product type.
    /// </summary>
    public string Name { get; set; } = null!;
}