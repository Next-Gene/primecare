using PrimeCare.Core.Entities;

namespace PrimeCare.Core.Specifications;

/// <summary>
/// Specification for products with their types and brands included.
/// </summary>
public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProductsWithTypesAndBrandsSpecification"/> class.
    /// </summary>
    public ProductsWithTypesAndBrandsSpecification()
    {
        AddInclude(x => x.ProductType);
        AddInclude(x => x.ProductBrand);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductsWithTypesAndBrandsSpecification"/> class with a specific product ID.
    /// </summary>
    /// <param name="id">The product identifier.</param>
    public ProductsWithTypesAndBrandsSpecification(int id)
        : base(x => x.Id == id)
    {
        AddInclude(x => x.ProductType);
        AddInclude(x => x.ProductBrand);
    }
}