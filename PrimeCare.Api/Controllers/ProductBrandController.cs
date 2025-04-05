using Microsoft.AspNetCore.Mvc;
using PrimeCare.Application.Dtos.ProductBrand;
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
    public async Task<IActionResult> GetProductBrands()
        => Ok(await _productBrandService.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductBrand(int id)
        => Ok(await _productBrandService.GetByIdAsync(id));

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
