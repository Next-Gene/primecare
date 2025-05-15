namespace PrimeCare.Shared.Dtos.Photos;

/// <summary>
/// Data Transfer Object representing a photo associated with a category.
/// </summary>
public class CategoryPhotoDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the category photo.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the URL where the photo is stored.
    /// </summary>
    public string Url { get; set; } = null!;

    /// <summary>
    /// Gets or sets a value indicating whether this photo is the main photo for the category.
    /// </summary>
    public bool IsMain { get; set; }
}
