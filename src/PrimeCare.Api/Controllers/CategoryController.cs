using Microsoft.AspNetCore.Mvc;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Shared.Dtos.Categories;
using PrimeCare.Shared.Errors;

namespace PrimeCare.Api.Controllers;

public class CategoryController : BaseApiController
{
    private readonly ICategoryService _category;
    private readonly IPhotoServies _photoServies;

    public CategoryController(ICategoryService category, IPhotoServies photoServies)
    {
        _category = category;
        _photoServies = photoServies;
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
    public async Task<IActionResult> Update(UpdateCategoryDto category)
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


    //[HttpPost("add-photo")]

    //public async Task<CategoryPhotoDto> AddPhoto(IFormFile file)
    //{



    //}
}
