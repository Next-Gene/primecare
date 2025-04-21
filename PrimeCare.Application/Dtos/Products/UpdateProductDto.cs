using PrimeCare.Core.Entities;

namespace PrimeCare.Application.Dtos.Products;

public class UpdateProductDto : BaseProductDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    
    public int Id { get; set; }
   
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
