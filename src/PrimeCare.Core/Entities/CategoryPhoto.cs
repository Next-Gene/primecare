namespace PrimeCare.Core.Entities;

/// <summary>
/// Represents a photo associated with a category in the system.
/// </summary>
public class CategoryPhoto : BaseEntity
{
    /// <summary>
    /// Gets or sets the URL where the photo is stored.
    /// </summary>

    public string? Url { get; set; }

    /// <summary>
    /// Gets or sets the public identifier from the cloud storage provider.
    /// </summary>
    public string PublicId { get; set; } = null!;


    /// <summary>
    /// Gets or sets a value indicating whether this photo is the main photo for the category.
    /// </summary>
    public bool? IsMain { get; set; }


    /// <summary>
    /// Gets or sets the foreign key of the associated category.
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Gets or sets the navigation property to the associated category.
    /// </summary>
    public virtual Category Category { get; set; } = null!;
}