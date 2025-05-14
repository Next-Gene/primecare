using Microsoft.AspNetCore.Mvc;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Shared.Dtos.Categories;
using PrimeCare.Shared.Dtos.Photos;
using PrimeCare.Shared.Errors;

namespace PrimeCare.Api.Controllers;

[ApiController]
[Route("api/v1/categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    /// <summary>
    /// Get all categories with optional sorting
    /// </summary>
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
    /// Get category by ID
    /// </summary>
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
    /// Create new category
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto category)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _categoryService.AddAsync(category);
        if (!result.Success)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    /// <summary>
    /// Update existing category
    /// </summary>
    [HttpPut("{id}")]
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
    /// Delete category by ID
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var result = await _categoryService.DeleteAsync(id);
        if (!result.Success)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    /// <summary>
    /// Add photo to category
    /// </summary>
    [HttpPost("{id}/photo")]
    public async Task<ActionResult<CategoryPhotoDto>> AddPhotoToCategory(int id, IFormFile file)
    {
        var photo = await _categoryService.AddPhotoAsync(id, file);
        if (photo == null)
            return BadRequest("Failed to add photo or category not found.");

        return CreatedAtAction(nameof(GetCategoryById), new { id }, photo);
    }
}
