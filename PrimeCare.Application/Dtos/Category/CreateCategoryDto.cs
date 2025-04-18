namespace PrimeCare.Application.Dtos.Category;

/// <summary>
/// Data Transfer Object for creating a category.
/// </summary>
public class CreateCategoryDto
{
    /// <summary>
    /// Gets or sets the name of the category.
    /// </summary>
    public string Name { get; set; } = null!;
}