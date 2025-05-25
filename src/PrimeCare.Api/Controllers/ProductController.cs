using Microsoft.AspNetCore.Mvc;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities;
using PrimeCare.Core.Interfaces;
using PrimeCare.Shared.Dtos.Photos;
using PrimeCare.Shared.Dtos.Products;

namespace PrimeCare.Api.Controllers;

/// <summary>
/// API controller for managing products, including CRUD operations and photo management.
/// </summary>
[ApiController]
[Route("api/v1/products")]
public class ProductController : BaseApiController
{
    private readonly IProductService _productService;
    private readonly IGenericRepository<Product> _productInterface;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductController"/> class.
    /// </summary>
    /// <param name="productService">The product service for product operations.</param>
    public ProductController(IProductService productService,
        IGenericRepository<Product> productInterface)
    {
        _productService = productService;
        _productInterface = productInterface;
    }

    /// <summary>
    /// Retrieves all products with optional filters and sorting.
    /// </summary>
    /// <param name="sort">The sorting criteria (optional).</param>
    /// <param name="brandId">The brand ID to filter by (optional).</param>
    /// <param name="categoryId">The category ID to filter by (optional).</param>
    /// <returns>A list of products matching the criteria, or a not found response if none exist.</returns>
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
    /// Retrieves a product by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <returns>The product if found; otherwise, a not found response.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProductById(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        return product != null ? Ok(product) : NotFound($"Product with ID {id} not found.");
    }

    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="product">The product data to create.</param>
    /// <returns>A result indicating success or failure.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto product)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _productService.AddAsync(product);
        return result.Success ? Ok(result.Message) : BadRequest(result.Message);
    }

    /// <summary>
    /// Updates an existing product.
    /// </summary>
    /// <param name="id">The unique identifier of the product to update.</param>
    /// <param name="product">The updated product data.</param>
    /// <returns>A result indicating success or failure.</returns>
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
    /// Deletes a product by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product to delete.</param>
    /// <returns>A result indicating success or failure.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var result = await _productService.DeleteAsync(id);
        return result.Success ? Ok(result.Message) : BadRequest(result.Message);
    }

    /// <summary>
    /// Adds a photo to a product.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <param name="file">The photo file to add.</param>
    /// <returns>The added photo or a bad request if the operation fails.</returns>
    [HttpPost("{id}/photo")]
    public async Task<ActionResult<ProductPhotoDto>> AddProductPhoto(int id, IFormFile file)
    {
        var photo = await _productService.AddPhotoAsync(id, file);
        if (photo == null)
            return BadRequest("Failed to add photo or product not found.");

        return CreatedAtAction(nameof(GetProductById), new { id }, photo);
    }

    [HttpPut("set-main-photo/{productId}/{photoId}")]
    public async Task<ActionResult> SetMainPhoto(int productId, int photoId)
    {
        var product = await _productInterface.GetByIdAsync(productId);
        if (product == null)
            return null!;

        var photo = product.ProductPhotos.FirstOrDefault(x => x.Id == photoId);
        if (photo!.IsMain) return BadRequest("This is already your main photo");

        var currentMain = product.ProductPhotos.FirstOrDefault(x => x.IsMain);
        if (currentMain != null) currentMain.IsMain = false;

        if (await _productInterface.SaveAllAsync()) return NoContent();

        return BadRequest("Faild to set main photo");
    }
}
