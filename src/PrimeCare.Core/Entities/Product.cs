namespace PrimeCare.Core.Entities;

/// <summary>
/// Represents a product.
/// </summary>
public class Product : BaseEntity
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
    /// Gets or sets the collection of photos associated with this product.
    /// </summary>
    public ICollection<ProductPhoto> ProductPhotos { get; set; } = new List<ProductPhoto>();


    /// <summary>
    /// Gets or sets the category of the product.
    /// </summary>
    public Category Category { get; set; } = null!;

    /// <summary>
    /// Gets or sets the identifier of the product category.
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Gets or sets the brand of the product.
    /// </summary>
    public ProductBrand ProductBrand { get; set; } = null!;

    /// <summary>
    /// Gets or sets the identifier of the product brand.
    /// </summary>
    public int ProductBrandId { get; set; }

    public string CreatedBy { get; set; }
    public string CreatedByName { get; set; }
}
