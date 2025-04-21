using PrimeCare.Core.Entities;

namespace PrimeCare.Application.Dtos.Products;

public class CreateProductDto:BaseProductDto
{


    /// <summary>
    /// Gets or sets the collection of photos associated with this category.
    /// Initialized as an empty list to avoid null reference exceptions.
    /// </summary>
    public ICollection<CategoryPhoto> CategoryPhoto { get; set; } = null!;


    /// <summary>
    /// img url   
    /// </summary>
    public string PhotoUrl { get; set; } = null!;

}
