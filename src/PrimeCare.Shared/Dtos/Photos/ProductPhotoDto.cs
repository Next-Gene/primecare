namespace PrimeCare.Shared.Dtos.Photos;

/// <summary>
/// Data Transfer Object representing a photo associated with a product.
/// </summary>
public class ProductPhotoDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the product photo.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the URL where the photo is stored.
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this photo is the main photo for the product.
    /// </summary>
    public bool IsMain { get; set; }
}
