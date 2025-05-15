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

    /// <summary>
    /// Gets or sets the slug of the category.
    /// </summary>
    public string Slug { get; set; } = null!;

    /// <summary>
    /// Gets or sets the date and time when the category was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the category was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the collection of photos associated with this category.
    /// Initialized as an empty list to avoid null reference exceptions.
    /// </summary>
    public ICollection<CategoryPhoto> CategoryPhotos { get; set; } = new List<CategoryPhoto>();

    /// <summary>
    /// Gets or sets the description of the category.
    /// </summary>
    public string Description { get; set; } = null!;
}
