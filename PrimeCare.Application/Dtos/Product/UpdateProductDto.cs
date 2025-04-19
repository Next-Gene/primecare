namespace PrimeCare.Application.Dtos.Product;

public class UpdateProductDto : BaseProductDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the type of the category.
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Gets or sets the brand of the product.
    /// </summary>
    public int ProductBrandId { get; set; }

}
