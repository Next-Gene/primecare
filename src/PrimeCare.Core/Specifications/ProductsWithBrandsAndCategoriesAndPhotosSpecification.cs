using PrimeCare.Core.Entities;

namespace PrimeCare.Core.Specifications;

/// <summary>
/// Specification for products with their Categories and brands included.
/// </summary>
public class ProductsWithBrandsAndCategoriesAndPhotosSpecification : BaseSpecification<Product>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProductsWithBrandsAndCategoriesAndPhotosSpecification"/> class.
    /// </summary>
    public ProductsWithBrandsAndCategoriesAndPhotosSpecification(string? sort)
        : base(null!)
    {
        AddInclude(x => x.Category);
        AddInclude(x => x.ProductBrand);
        AddInclude(x => x.ProductPhotos);
        AddOrderBy(x => x.Name);

        if (!string.IsNullOrEmpty(sort))
        {
            switch (sort)
            {
                case "priceAsc":
                    AddOrderBy(p => p.Price);
                    break;
                case "priceDesc":
                    AddOrderByDescending(p => p.Price);
                    break;
                default:
                    AddOrderBy(p => p.Name);
                    break;
            }
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductsWithBrandsAndCategoriesAndPhotosSpecification"/> class with a specific product ID.
    /// </summary>
    /// <param name="id">The product identifier.</param>
    public ProductsWithBrandsAndCategoriesAndPhotosSpecification(int id)
        : base(x => x.Id == id)
    {
        AddInclude(x => x.Category);
        AddInclude(x => x.ProductBrand);
        AddInclude(x => x.ProductPhotos);

    }



}