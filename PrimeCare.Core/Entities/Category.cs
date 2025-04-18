namespace PrimeCare.Core.Entities;

/// <summary>
/// Represents a category of product.
/// </summary>
public class Category : BaseEntity
{
    /// <summary>
    /// Gets or sets the name of the category.
    /// </summary>
    public string Name { get; set; } = null!;
}