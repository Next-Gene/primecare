using Microsoft.AspNetCore.Mvc;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Shared.Dtos.Photos;
using PrimeCare.Shared.Dtos.Products;

namespace PrimeCare.Api.Controllers;

[ApiController]
[Route("api/v1/products")]
public class ProductController : BaseApiController
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// Get all products with optional filters and sorting.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetAllProducts(
        string? sort, int? brandId, int? categoryId)
    {
        var products = await _productService.GetAllAsync(sort, brandId, categoryId);
        if (!products.Any())
            return NotFound("No products found matching the criteria.");

        return Ok(products);
    }

    /// <summary>
    /// Get product by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProductById(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        return product != null ? Ok(product) : NotFound($"Product with ID {id} not found.");
    }

    /// <summary>
    /// Create new product
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto product)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _productService.AddAsync(product);
        return result.Success ? Ok(result.Message) : BadRequest(result.Message);
    }

    /// <summary>
    /// Update product
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto product)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        product.Id = id;
        var result = await _productService.UpdateAsync(product);
        return result.Success ? Ok(result.Message) : BadRequest(result.Message);
    }

    /// <summary>
    /// Delete product by ID
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var result = await _productService.DeleteAsync(id);
        return result.Success ? Ok(result.Message) : BadRequest(result.Message);
    }

    /// <summary>
    /// Add photo to product
    /// </summary>
    [HttpPost("{id}/photo")]
    public async Task<ActionResult<ProductPhotoDto>> AddProductPhoto(int id, IFormFile file)
    {
        var photo = await _productService.AddPhotoAsync(id, file);
        if (photo == null)
            return BadRequest("Failed to add photo or product not found.");

        return CreatedAtAction(nameof(GetProductById), new { id }, photo);
    }
}
