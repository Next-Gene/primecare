using Microsoft.AspNetCore.Mvc;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Shared.Dtos.Categories;
using PrimeCare.Shared.Dtos.Photos;
using PrimeCare.Shared.Errors;

namespace PrimeCare.Api.Controllers;

/// <summary>
/// Controller for managing categories, providing endpoints for CRUD operations and photo management.
/// </summary>
public class CategoryController : BaseApiController
{
    private readonly ICategoryService _categoryService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryController"/> class.
    /// </summary>
    /// <param name="categoryService">The service for managing categories.</param>
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    /// <summary>
    /// Retrieves all categories with optional sorting.
    /// </summary>
    /// <param name="sort">The sorting criteria (e.g., "nameAsc", "nameDesc").</param>
    /// <returns>
    /// A list of categories or a 404 status if no categories are found.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCategories(string? sort)
    {
        var categories = await _categoryService.GetAllAsync(sort);

        if (!categories.Any())
            return NotFound("No Categories found matching the criteria.");

        return Ok(categories);
    }

    /// <summary>
    /// Retrieves a specific category by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the category.</param>
    /// <returns>
    /// The category details or a 404 status if the category is not found.
    /// </returns>
    [HttpGet("{id}", Name = "GetCategory")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCategory(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null)
            return NotFound($"Category with ID {id} not found.");

        return Ok(category);
    }

    /// <summary>
    /// Adds a new category.
    /// </summary>
    /// <param name="category">The category data to add.</param>
    /// <returns>
    /// A success message or a 400 status if the operation fails.
    /// </returns>
    [HttpPost("add")]
    public async Task<IActionResult> Add(CreateCategoryDto category)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _categoryService.AddAsync(category);

        if (!result.Success)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    /// <summary>
    /// Updates an existing category.
    /// </summary>
    /// <param name="category">The updated category data.</param>
    /// <returns>
    /// A success message or a 400 status if the operation fails.
    /// </returns>
    [HttpPut("update")]
    public async Task<IActionResult> Update(UpdateCategoryDto category)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _categoryService.UpdateAsync(category);

        if (!result.Success) return BadRequest(result.Message);

        return Ok(result.Message);
    }

    /// <summary>
    /// Deletes a category by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the category to delete.</param>
    /// <returns>
    /// A success message or a 400 status if the operation fails.
    /// </returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _categoryService.DeleteAsync(id);

        if (!result.Success)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    /// <summary>
    /// Adds a photo to a specific category.
    /// </summary>
    /// <param name="id">The unique identifier of the category.</param>
    /// <param name="file">The photo file to add.</param>
    /// <returns>
    /// The added photo details or a 400 status if the operation fails.
    /// </returns>
    [HttpPost("add-photo/{id}")]
    public async Task<ActionResult<CategoryPhotoDto>> AddPhoto(int id, IFormFile file)
    {
        var photo = await _categoryService.AddPhotoAsync(id, file);
        if (photo == null)
            return BadRequest("Failed to add photo or category not found.");

        return CreatedAtRoute("GetCategory", new { id }, photo);
    }
}