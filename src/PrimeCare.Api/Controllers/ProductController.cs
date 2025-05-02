using Microsoft.AspNetCore.Mvc;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Shared.Dtos.Photos;
using PrimeCare.Shared.Dtos.Products;

namespace PrimeCare.Api.Controllers;

public class ProductController : BaseApiController
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// Retrieves a list of products with optional sorting and filtering.
    /// </summary>
    /// <param name="sort">Sorting criteria (e.g., "priceAsc", "priceDesc").</param>
    /// <param name="brandId">Filter by brand ID.</param>
    /// <param name="categoryId">Filter by category ID.</param>
    /// <returns>A list of products or a 404 status if no products are found.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetProducts(
        string? sort, int? brandId, int? categoryId)
    {
        var products = await _productService.GetAllAsync(sort, brandId, categoryId);
        if (!products.Any())
            return NotFound("No products found matching the criteria.");

        return Ok(products);
    }

    /// <summary>
    /// Retrieves a specific product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product.</param>
    /// <returns>The product details or a 404 status if not found.</returns>
    [HttpGet("{id}", Name = "GetProduct")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
            return NotFound($"Product with ID {id} not found.");

        return Ok(product);
    }

    /// <summary>
    /// Adds a new product.
    /// </summary>
    /// <param name="product">The product details to add.</param>
    /// <returns>A success message or a 400 status if the operation fails.</returns>
    [HttpPost("add")]
    public async Task<IActionResult> Add(CreateProductDto product)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _productService.AddAsync(product);
        if (!result.Success)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    /// <summary>
    /// Updates an existing product.
    /// </summary>
    /// <param name="product">The updated product details.</param>
    /// <returns>A success message or a 400 status if the operation fails.</returns>
    [HttpPut("update")]
    public async Task<IActionResult> Update(UpdateProductDto product)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _productService.UpdateAsync(product);
        if (!result.Success)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    /// <summary>
    /// Deletes a product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product to delete.</param>
    /// <returns>A success message or a 400 status if the operation fails.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _productService.DeleteAsync(id);
        if (!result.Success)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    /// <summary>
    /// Adds a photo to a specific product.
    /// </summary>
    /// <param name="id">The ID of the product.</param>
    /// <param name="file">The photo file to add.</param>
    /// <returns>The added photo details or a 400 status if the operation fails.</returns>
    [HttpPost("add-photo/{id}")]
    public async Task<ActionResult<ProductPhotoDto>> AddPhoto(int id, IFormFile file)
    {
        var photo = await _productService.AddPhotoAsync(id, file);
        if (photo == null)
            return BadRequest("Failed to add photo or product not found.");

        return CreatedAtRoute("GetProduct", new { id }, photo);
    }
}