using Microsoft.AspNetCore.Http;
using PrimeCare.Shared;
using PrimeCare.Shared.Dtos.Photos;
using PrimeCare.Shared.Dtos.Products;

namespace PrimeCare.Application.Services.Interfaces;

/// <summary>
/// Defines the contract for product-related operations such as retrieving, creating, updating, deleting products, and managing product photos.
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Retrieves a product by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <returns>The <see cref="ProductDto"/> representing the product.</returns>
    Task<ProductDto> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves all products, optionally filtered and sorted by the specified criteria.
    /// </summary>
    /// <param name="sort">The sorting criteria (optional).</param>
    /// <param name="brandId">The identifier of the product brand to filter by (optional).</param>
    /// <param name="categoryId">The identifier of the product category to filter by (optional).</param>
    /// <returns>A read-only list of <see cref="ProductDto"/> objects.</returns>
    Task<IReadOnlyList<ProductDto>> GetAllAsync(string? sort, int? brandId, int? categoryId);

    /// <summary>
    /// Adds a new product.
    /// </summary>
    /// <param name="entity">The <see cref="CreateProductDto"/> containing the product data.</param>
    /// <returns>A <see cref="ServiceResponse"/> indicating the result of the operation.</returns>
    Task<ServiceResponse> AddAsync(CreateProductDto entity);

    /// <summary>
    /// Updates an existing product.
    /// </summary>
    /// <param name="entity">The <see cref="UpdateProductDto"/> containing the updated product data.</param>
    /// <returns>A <see cref="ServiceResponse"/> indicating the result of the operation.</returns>
    Task<ServiceResponse> UpdateAsync(UpdateProductDto entity);

    /// <summary>
    /// Deletes a product by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product to delete.</param>
    /// <returns>A <see cref="ServiceResponse"/> indicating the result of the operation.</returns>
    Task<ServiceResponse> DeleteAsync(int id);

    /// <summary>
    /// Adds a photo to a product.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <param name="file">The photo file to add.</param>
    /// <returns>The <see cref="ProductPhotoDto"/> representing the added photo, or <c>null</c> if the operation failed.</returns>
    Task<ProductPhotoDto?> AddPhotoAsync(int productId, IFormFile file);
}
