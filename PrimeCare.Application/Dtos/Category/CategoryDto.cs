namespace PrimeCare.Application.Dtos.Category;

/// <summary>
/// Data Transfer Object for a category.
/// </summary>
public class CategoryDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the category.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the category.
    /// </summary>
    public string Name { get; set; } = null!;
}