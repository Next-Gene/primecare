using Microsoft.AspNetCore.Http;
using PrimeCare.Shared;
using PrimeCare.Shared.Dtos.Categories;
using PrimeCare.Shared.Dtos.Photos;

namespace PrimeCare.Application.Services.Interfaces;

/// <summary>
/// Defines the contract for category-related operations such as retrieving, creating, updating, deleting categories, and managing category photos.
/// </summary>
public interface ICategoryService
{
    /// <summary>
    /// Retrieves a category by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the category.</param>
    /// <returns>The <see cref="CategoryDto"/> representing the category.</returns>
    Task<CategoryDto> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves all categories, optionally sorted by the specified criteria.
    /// </summary>
    /// <param name="sort">The sorting criteria (optional).</param>
    /// <returns>A read-only list of <see cref="CategoryDto"/> objects.</returns>
    Task<IReadOnlyList<CategoryDto>> GetAllAsync(string? sort);

    /// <summary>
    /// Adds a new category.
    /// </summary>
    /// <param name="entity">The <see cref="CreateCategoryDto"/> containing the category data.</param>
    /// <returns>A <see cref="ServiceResponse"/> indicating the result of the operation.</returns>
    Task<ServiceResponse> AddAsync(CreateCategoryDto entity);

    /// <summary>
    /// Updates an existing category.
    /// </summary>
    /// <param name="entity">The <see cref="UpdateCategoryDto"/> containing the updated category data.</param>
    /// <returns>A <see cref="ServiceResponse"/> indicating the result of the operation.</returns>
    Task<ServiceResponse> UpdateAsync(UpdateCategoryDto entity);

    /// <summary>
    /// Deletes a category by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the category to delete.</param>
    /// <returns>A <see cref="ServiceResponse"/> indicating the result of the operation.</returns>
    Task<ServiceResponse> DeleteAsync(int id);

    /// <summary>
    /// Adds a photo to a category.
    /// </summary>
    /// <param name="categoryId">The unique identifier of the category.</param>
    /// <param name="file">The photo file to add.</param>
    /// <returns>The <see cref="CategoryPhotoDto"/> representing the added photo, or <c>null</c> if the operation failed.</returns>
    Task<CategoryPhotoDto?> AddPhotoAsync(int categoryId, IFormFile file);
}
