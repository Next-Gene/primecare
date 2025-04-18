using Microsoft.AspNetCore.Mvc;
using PrimeCare.Application.Dtos.Category;
using PrimeCare.Application.Errors;
using PrimeCare.Application.Services.Interfaces;

namespace PrimeCare.Api.Controllers;

public class CategoryController : BaseApiController
{
    private readonly ICategoryService _category;

    public CategoryController(ICategoryService category)
    {
        _category = category;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _category.GetAllAsync();
        return categories.Any() ? Ok(categories) : NotFound(categories);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCategory(int id)
    {
        var category = await _category.GetByIdAsync(id);
        return category != null ? Ok(category) : NotFound(category);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add(CreateCategoryDto category)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _category.AddAsync(category);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update(CategoryDto category)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _category.UpdateAsync(category);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _category.DeleteAsync(id);

        return result.Success ? Ok(result) : BadRequest(result);
    }
}
