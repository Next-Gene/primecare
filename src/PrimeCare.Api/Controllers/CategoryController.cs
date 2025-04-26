using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities;
using PrimeCare.Core.Interfaces;
using PrimeCare.Shared.Dtos.Categories;
using PrimeCare.Shared.Dtos.Photos;
using PrimeCare.Shared.Errors;

namespace PrimeCare.Api.Controllers;

public class CategoryController : BaseApiController
{
    private readonly ICategoryService _categoryService;
    private readonly IPhotoService _photoService;
    private readonly IGenericRepository<Category> _categoryRepository;
    private readonly IMapper _mapper;
    public CategoryController(ICategoryService categoryService,
        IGenericRepository<Category> categoryRepository, IMapper mapper,
        IPhotoService photoService)
    {
        _categoryService = categoryService;
        _categoryRepository = categoryRepository;
        _photoService = photoService;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _categoryService.GetAllAsync();
        return categories.Any() ? Ok(categories) : NotFound(categories);
    }

    [HttpGet("{id}", Name = "GetCategory")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCategory(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        return category != null ? Ok(category) : NotFound(category);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add(CreateCategoryDto category)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _categoryService.AddAsync(category);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update(UpdateCategoryDto category)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _categoryService.UpdateAsync(category);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _categoryService.DeleteAsync(id);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("add-photo/{id}")]
    public async Task<ActionResult<CategoryPhotoDto>> AddPhoto(int id, IFormFile file)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
            return NotFound($"Category with id {id} not found");
        var result = await _photoService.AddPhotoAsync(file);

        if (result.Error != null) return BadRequest(result.Error.Message);

        var categoryPhoto = new CategoryPhoto
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        if (category.CategoryPhotos.Count == 0)
        {
            categoryPhoto.IsMain = true;
        }


        category.CategoryPhotos.Add(categoryPhoto);
        if (await _categoryRepository.SaveAllAsync())
            return CreatedAtRoute("GetCategory", new { id = category.Id }, _mapper.Map<CategoryPhotoDto>(categoryPhoto));
        return BadRequest("Problem Adding Photo");
    }

    //[HttpDelete("photo")]
    //public async Task<IActionResult> DeletePhoto(int categoryId, string publicId)
    //{
    //    var result = await _category.DeletePhotoAsync(categoryId, publicId);
    //    return result.Success ? Ok(result) : BadRequest(result);
    //}
}
