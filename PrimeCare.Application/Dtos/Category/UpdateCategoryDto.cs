
using PrimeCare.Core.Entities;

namespace PrimeCare.Application.Dtos.Category;

public class UpdateCategoryDto:BaseEntity
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
    /// Gets or sets the image path of the category.
    /// </summary>
    public string ImageUrl { get; set; } = null!;



    /// <summary>
    /// Gets or sets the date and time when the category was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }



    /// <summary>
    /// Gets or sets the description of the category.
    /// </summary>
    public string Description { get; set; } = null!;
}
