namespace PrimeCare.Shared.Dtos.Categories;

/// <summary>
/// Data Transfer Object for creating a category.
/// </summary>
public class CreateCategoryDto
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
    /// Gets or sets the description of the category.
    /// </summary>
    public string Description { get; set; } = null!;
}