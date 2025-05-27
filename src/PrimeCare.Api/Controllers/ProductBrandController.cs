using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Shared.Dtos.ProductBrand;
using PrimeCare.Shared.Errors;

namespace PrimeCare.Api.Controllers;

/// <summary>
/// API controller for managing product brands.
/// </summary>
[ApiController]
[Route("api/v1/product-brands")]
public class ProductBrandController : BaseApiController
{
    private readonly IProductBrandService _productBrandService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductBrandController"/> class.
    /// </summary>
    /// <param name="productBrandService">The product brand service for brand operations.</param>
    public ProductBrandController(IProductBrandService productBrandService)
    {
        _productBrandService = productBrandService;
    }

    /// <summary>
    /// Retrieves all product brands.
    /// Public access - no authorization required.
    /// </summary>
    /// <returns>A list of product brands or 404 if none found.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllProductBrands()
    {
        var productBrands = await _productBrandService.GetAllAsync();
        return productBrands.Any() ? Ok(productBrands) : NotFound("No product brands found.");
    }

    /// <summary>
    /// Retrieves a specific product brand by ID.
    /// Public access - no authorization required.
    /// </summary>
    /// <param name="id">The ID of the product brand.</param>
    /// <returns>The product brand details or 404 if not found.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductBrandById(int id)
    {
        var productBrand = await _productBrandService.GetByIdAsync(id);
        return productBrand != null ? Ok(productBrand) : NotFound($"Product brand with ID {id} not found.");
    }

    /// <summary>
    /// Creates a new product brand.
    /// Requires Admin or Seller role.
    /// </summary>
    /// <param name="productBrand">The product brand data.</param>
    /// <returns>Result of the creation operation.</returns>
    [HttpPost]
    [Authorize(Policy = "AdminOrSeller")]
    public async Task<IActionResult> CreateProductBrand([FromBody] CreateProductBrandDto productBrand)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _productBrandService.AddAsync(productBrand);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Updates an existing product brand.
    /// Requires Admin or Seller role.
    /// </summary>
    /// <param name="id">The ID of the product brand to update.</param>
    /// <param name="productBrand">The updated product brand data.</param>
    /// <returns>Result of the update operation.</returns>
    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOrSeller")]
    public async Task<IActionResult> UpdateProductBrand(int id, [FromBody] ProductBrandDto productBrand)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        productBrand.Id = id;
        var result = await _productBrandService.UpdateAsync(productBrand);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Deletes a product brand by ID.
    /// Requires Admin role only.
    /// </summary>
    /// <param name="id">The ID of the product brand to delete.</param>
    /// <returns>Result of the delete operation.</returns>
    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteProductBrand(int id)
    {
        var result = await _productBrandService.DeleteAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}