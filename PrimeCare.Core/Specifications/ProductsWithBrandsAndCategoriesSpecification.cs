using PrimeCare.Core.Entities;

namespace PrimeCare.Core.Specifications;

/// <summary>
/// Specification for products with their Categories and brands included.
/// </summary>
public class ProductsWithBrandsAndCategoriesSpecification : BaseSpecification<Product>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProductsWithBrandsAndCategoriesSpecification"/> class.
    /// </summary>
    public ProductsWithBrandsAndCategoriesSpecification() : base(null!)
    {
        AddInclude(x => x.Category);
        AddInclude(x => x.ProductBrand);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductsWithBrandsAndCategoriesSpecification"/> class with a specific product ID.
    /// </summary>
    /// <param name="id">The product identifier.</param>
    public ProductsWithBrandsAndCategoriesSpecification(int id)
        : base(x => x.Id == id)
    {
        AddInclude(x => x.Category);
        AddInclude(x => x.ProductBrand);
    }
}