using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Shared.Dtos.Categories;
using PrimeCare.Shared.Dtos.Photos;
using PrimeCare.Shared.Errors;

namespace PrimeCare.Api.Controllers;

/// <summary>
/// API controller for managing product categories.
/// </summary>
[ApiController]
[Route("api/v1/categories")]
public class CategoryController : BaseApiController
{
    private readonly ICategoryService _categoryService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryController"/> class.
    /// </summary>
    /// <param name="categoryService">The category service for category operations.</param>
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    /// <summary>
    /// Retrieves all categories with optional sorting.
    /// Public access - no authorization required.
    /// </summary>
    /// <param name="sort">The sorting criteria (optional).</param>
    /// <returns>A list of categories or a not found response if none exist.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllCategories([FromQuery] string? sort)
    {
        var categories = await _categoryService.GetAllAsync(sort);
        if (!categories.Any())
            return NotFound("No categories found matching the criteria.");
        return Ok(categories);
    }

    /// <summary>
    /// Retrieves a category by its unique identifier.
    /// Public access - no authorization required.
    /// </summary>
    /// <param name="id">The unique identifier of the category.</param>
    /// <returns>The category if found; otherwise, a not found response.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null)
            return NotFound($"Category with ID {id} not found.");
        return Ok(category);
    }

    /// <summary>
    /// Creates a new category.
    /// Requires Admin or Seller role.
    /// </summary>
    /// <param name="category">The category data to create.</param>
    /// <returns>A result indicating success or failure.</returns>
    [HttpPost]
    [Authorize(Policy = "AdminOrSeller")]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto category)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _categoryService.AddAsync(category);
        if (!result.Success)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    /// <summary>
    /// Updates an existing category.
    /// Requires Admin or Seller role.
    /// </summary>
    /// <param name="id">The unique identifier of the category to update.</param>
    /// <param name="category">The updated category data.</param>
    /// <returns>A result indicating success or failure.</returns>
    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOrSeller")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto category)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        category.Id = id; // Ensure ID is passed
        var result = await _categoryService.UpdateAsync(category);
        if (!result.Success)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    /// <summary>
    /// Deletes a category by its unique identifier.
    /// Requires Admin role only.
    /// </summary>
    /// <param name="id">The unique identifier of the category to delete.</param>
    /// <returns>A result indicating success or failure.</returns>
    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var result = await _categoryService.DeleteAsync(id);
        if (!result.Success)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    /// <summary>
    /// Adds a photo to a category.
    /// Requires Admin or Seller role.
    /// </summary>
    /// <param name="id">The unique identifier of the category.</param>
    /// <param name="file">The photo file to add.</param>
    /// <returns>The added photo or a bad request if the operation fails.</returns>
    [HttpPost("{id}/photo")]
    [Authorize(Policy = "AdminOrSeller")]
    public async Task<ActionResult<CategoryPhotoDto>> AddPhotoToCategory(int id, IFormFile file)
    {
        var photo = await _categoryService.AddPhotoAsync(id, file);
        if (photo == null)
            return BadRequest("Failed to add photo or category not found.");

        return CreatedAtAction(nameof(GetCategoryById), new { id }, photo);
    }
}