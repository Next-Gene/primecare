using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrimeCare.Application.Helpers;
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
    private readonly IPhotoService _photoService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductController"/> class.
    /// </summary>
    /// <param name="productService">The product service for product operations.</param>
    public ProductController(IProductService productService,
        IPhotoService photoService,
        IGenericRepository<Product> productInterface)
    {
        _productService = productService;
        _photoService = photoService;
        _productInterface = productInterface;
    }

    /// <summary>
    /// Retrieves all products with optional filters and sorting.
    /// Public access - no authorization required.
    /// </summary>
    /// <param name="sort">The sorting criteria (optional).</param>
    /// <param name="brandId">The brand ID to filter by (optional).</param>
    /// <param name="categoryId">The category ID to filter by (optional).</param>
    /// <returns>A list of products matching the criteria, or a not found response if none exist.</returns>


    [Cashed(600)]
    
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
    /// Public access - no authorization required.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <returns>The product if found; otherwise, a not found response.</returns>
    [Cashed(600)]
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProductById(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        return product != null ? Ok(product) : NotFound($"Product with ID {id} not found.");
    }

    /// <summary>
    /// Creates a new product.
    /// Requires Admin or Seller role.
    /// </summary>
    /// <param name="product">The product data to create.</param>
    /// <returns>A result indicating success or failure.</returns>
    [HttpPost]
    [Authorize(Policy = "AdminOrSeller")]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto product)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _productService.AddAsync(product);
        return result.Success ? Ok(result.Message) : BadRequest(result.Message);
    }

    /// <summary>
    /// Updates an existing product.
    /// Requires Admin or Seller role.
    /// </summary>
    /// <param name="id">The unique identifier of the product to update.</param>
    /// <param name="product">The updated product data.</param>
    /// <returns>A result indicating success or failure.</returns>
    
    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOrSeller")]
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
    /// Requires Admin role only.
    /// </summary>
    /// <param name="id">The unique identifier of the product to delete.</param>
    /// <returns>A result indicating success or failure.</returns>
    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var result = await _productService.DeleteAsync(id);
        return result.Success ? Ok(result.Message) : BadRequest(result.Message);
    }

    /// <summary>
    /// Adds a photo to a product.
    /// Requires Admin or Seller role.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <param name="file">The photo file to add.</param>
    /// <returns>The added photo or a bad request if the operation fails.</returns>
    [HttpPost("{id}/photo")]
    [Authorize(Policy = "AdminOrSeller")]
    public async Task<ActionResult<ProductPhotoDto>> AddProductPhoto(int id, IFormFile file)
    {
        var photo = await _productService.AddPhotoAsync(id, file);
        if (photo == null)
            return BadRequest("Failed to add photo or product not found.");

        return CreatedAtAction(nameof(GetProductById), new { id }, photo);
    }

    /// <summary>
    /// Sets a photo as the main photo for a product.
    /// Requires Admin or Seller role.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <param name="photoId">The unique identifier of the photo to set as main.</param>
    /// <returns>A result indicating success or failure.</returns>
    [HttpPut("set-main-photo/{productId}/{photoId}")]
    [Authorize(Policy = "AdminOrSeller")]
    public async Task<ActionResult> SetMainPhoto(int productId, int photoId)
    {
        var product = await _productInterface.GetByIdAsync(productId);
        if (product == null)
            return NotFound($"Product with ID {productId} not found.");

        var photo = product.ProductPhotos.FirstOrDefault(x => x.Id == photoId);
        if (photo == null)
            return NotFound($"Photo with ID {photoId} not found for this product.");

        if (photo.IsMain)
            return BadRequest("This is already your main photo");

        var currentMain = product.ProductPhotos.FirstOrDefault(x => x.IsMain);
        if (currentMain != null)
            currentMain.IsMain = false;

        photo.IsMain = true;

        if (await _productInterface.SaveAllAsync())
            return NoContent();

        return BadRequest("Failed to set main photo");
    }

    [HttpDelete("delete-photo/{photoId}/{productId}")]
    [Authorize(Policy = "AdminOrSeller")]
    public async Task<ActionResult> DeletePhoto(int photoId, int productId)
    {
        var product = await _productInterface.GetByIdAsync(productId);
        if (product == null)
            return NotFound($"Product with ID {productId} not found.");

        var photo = product.ProductPhotos.FirstOrDefault(x => x.Id == photoId);
        if (photo == null)
            return NotFound($"Photo with ID {photoId} not found for this product.");

        if (photo.IsMain)
            return BadRequest("You cannot delete the main photo of a product.");

        if (photo.PublicId != null)
        {
            var result = await _photoService.DeletePhotoAsync(photo.PublicId);
            if (result.Error != null)
                return BadRequest("Failed to delete photo from cloud storage.");
        }

        product.ProductPhotos.Remove(photo);

        if (await _productInterface.SaveAllAsync())
            return Ok();

        return BadRequest("Failed to delete photo from product.");
    }
}