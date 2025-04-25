using Microsoft.AspNetCore.Mvc;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Shared.Dtos.Products;
using PrimeCare.Shared.Errors;
namespace PrimeCare.Api.Controllers;

public class ProductController : BaseApiController
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _productService.GetAllAsync();
        return products.Any() ? Ok(products) : NotFound(products);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProduct(int id)
    {
        var data = await _productService.GetByIdAsync(id);
        return data != null ? Ok(data) : NotFound(data);
    }


    [HttpPost("add")]
    public async Task<IActionResult> Add(CreateProductDto Product)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _productService.AddAsync(Product);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update(UpdateProductDto product)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _productService.UpdateAsync(product);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _productService.DeleteAsync(id);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("photo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> AddPhoto(int productId, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest();
        }

        var result = await _productService.AddPhotoAsync(productId, file);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("photo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeletePhoto(int productId, string publicId)
    {
        var result = await _productService.DeletePhotoAsync(productId, publicId);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}



