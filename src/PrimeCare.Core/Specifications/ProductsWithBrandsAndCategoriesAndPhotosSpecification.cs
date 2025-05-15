using PrimeCare.Core.Entities;

namespace PrimeCare.Core.Specifications;

/// <summary>
/// A specification for retrieving products with their associated brands, categories, and photos.
/// This specification supports filtering by brand and category, as well as sorting by name or price.
/// </summary>
public class ProductsWithBrandsAndCategoriesAndPhotosSpecification : BaseSpecification<Product>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProductsWithBrandsAndCategoriesAndPhotosSpecification"/> class.
    /// </summary>
    /// <param name="sort">
    /// The sorting criteria. Supported values are "priceAsc" for ascending price and "priceDesc" for descending price. 
    /// Defaults to sorting by name.
    /// </param>
    /// <param name="brandId">
    /// The ID of the product brand to filter by. If <c>null</c>, no filtering by brand is applied.
    /// </param>
    /// <param name="categoryId">
    /// The ID of the product category to filter by. If <c>null</c>, no filtering by category is applied.
    /// </param>
    public ProductsWithBrandsAndCategoriesAndPhotosSpecification(
        string? sort, int? brandId, int? categoryId)
        : base(product =>
            (!brandId.HasValue || product.ProductBrandId == brandId) &&
            (!categoryId.HasValue || product.CategoryId == categoryId))
    {
        // Include related entities
        AddInclude(x => x.Category);
        AddInclude(x => x.ProductBrand);
        AddInclude(x => x.ProductPhotos);

        // Default sorting by name
        AddOrderBy(x => x.Name);

        // Apply sorting based on the provided criteria
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
    /// Initializes a new instance of the <see cref="ProductsWithBrandsAndCategoriesAndPhotosSpecification"/> class
    /// for retrieving a specific product by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    public ProductsWithBrandsAndCategoriesAndPhotosSpecification(int id)
        : base(x => x.Id == id)
    {
        // Include related entities
        AddInclude(x => x.Category);
        AddInclude(x => x.ProductBrand);
        AddInclude(x => x.ProductPhotos);
    }
}
