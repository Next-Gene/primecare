using Microsoft.AspNetCore.Mvc;
using PrimeCare.Application.Dtos.ProductBrand;
using PrimeCare.Application.Errors;
using PrimeCare.Application.Services.Interfaces;

namespace PrimeCare.Api.Controllers;

public class ProductBrandController : BaseApiController
{
    private readonly IProductBrandService _productBrandService;

    public ProductBrandController(IProductBrandService productBrandService)
    {
        _productBrandService = productBrandService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductBrands()
    {
        var productbrands = await _productBrandService.GetAllAsync();
        return productbrands.Any() ? Ok(productbrands) : NotFound(productbrands);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductBrand(int id)
    {
        var productbrand = await _productBrandService.GetByIdAsync(id);
        return productbrand != null ? Ok(productbrand) : NotFound(productbrand);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add(CreateProductBrandDto productBrand)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _productBrandService.AddAsync(productBrand);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update(ProductBrandDto productBrand)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _productBrandService.UpdateAsync(productBrand);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _productBrandService.DeleteAsync(id);

        return result.Success ? Ok(result) : BadRequest(result);
    }
}
