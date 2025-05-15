using PrimeCare.Shared.Dtos.Photos;

namespace PrimeCare.Shared.Dtos.Categories;

/// <summary>
/// Data Transfer Object for updating a category.
/// </summary>
public class UpdateCategoryDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the category.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the category.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the slug of the category.
    /// </summary>
    public string Slug { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of photos associated with this category.
    /// Initialized as an empty list to avoid null reference exceptions.
    /// </summary>
    public ICollection<CategoryPhotoDto> CategoryPhoto { get; set; } = null!;

    /// <summary>
    /// Gets or sets the main image URL for the category.
    /// </summary>
    public string PhotoUrl { get; set; } = null!;

    /// <summary>
    /// Gets or sets the date and time when the category was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the description of the category.
    /// </summary>
    public string Description { get; set; } = null!;
}
