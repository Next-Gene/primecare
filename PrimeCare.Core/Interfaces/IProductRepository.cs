using PrimeCare.Core.Entities;

namespace PrimeCare.Core.Interfaces;

/// <summary>
/// Interface for managing product data.
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Gets a product by its identifier.
    /// </summary>
    /// <param name="id">The product identifier.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the product.</returns>
    Task<Product?> GetProductByIdAsync(int id);

    /// <summary>
    /// Gets all products.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of products.</returns>
    Task<IReadOnlyList<Product>> GetProductsAsync();

    /// <summary>
    /// Gets all product brands.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of product brands.</returns>
    Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync();

    /// <summary>
    /// Gets all product types.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of product types.</returns>
    Task<IReadOnlyList<ProductType>> GetProductTypesAsync();
}