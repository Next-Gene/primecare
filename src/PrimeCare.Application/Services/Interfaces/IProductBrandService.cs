using PrimeCare.Shared;
using PrimeCare.Shared.Dtos.ProductBrand;

namespace PrimeCare.Application.Services.Interfaces;

/// <summary>
/// Defines the contract for product brand-related operations such as retrieving, creating, updating, and deleting product brands.
/// </summary>
public interface IProductBrandService
{
    /// <summary>
    /// Retrieves a product brand by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product brand.</param>
    /// <returns>The <see cref="ProductBrandDto"/> representing the product brand.</returns>
    Task<ProductBrandDto> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves all product brands.
    /// </summary>
    /// <returns>A read-only list of <see cref="ProductBrandDto"/> objects.</returns>
    Task<IReadOnlyList<ProductBrandDto>> GetAllAsync();

    /// <summary>
    /// Adds a new product brand.
    /// </summary>
    /// <param name="entity">The <see cref="CreateProductBrandDto"/> containing the product brand data.</param>
    /// <returns>A <see cref="ServiceResponse"/> indicating the result of the operation.</returns>
    Task<ServiceResponse> AddAsync(CreateProductBrandDto entity);

    /// <summary>
    /// Updates an existing product brand.
    /// </summary>
    /// <param name="entity">The <see cref="ProductBrandDto"/> containing the updated product brand data.</param>
    /// <returns>A <see cref="ServiceResponse"/> indicating the result of the operation.</returns>
    Task<ServiceResponse> UpdateAsync(ProductBrandDto entity);

    /// <summary>
    /// Deletes a product brand by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product brand to delete.</param>
    /// <returns>A <see cref="ServiceResponse"/> indicating the result of the operation.</returns>
    Task<ServiceResponse> DeleteAsync(int id);
}
