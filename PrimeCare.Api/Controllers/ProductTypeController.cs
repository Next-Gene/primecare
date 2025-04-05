using Microsoft.AspNetCore.Mvc;
using PrimeCare.Application.Dtos.ProductBrand;
using PrimeCare.Application.Services.Interfaces;

namespace PrimeCare.Api.Controllers;

public class ProductTypeController : BaseApiController
{
    private readonly IProductTypeService _productTypeService;

    public ProductTypeController(IProductTypeService productTypeService)
    {
        _productTypeService = productTypeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProductTypes()
        => Ok(await _productTypeService.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductType(int id)
        => Ok(await _productTypeService.GetByIdAsync(id));

    [HttpPost("add")]
    public async Task<IActionResult> Add(CreateProductTypeDto productType)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _productTypeService.AddAsync(productType);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update(ProductTypeDto productType)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _productTypeService.UpdateAsync(productType);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _productTypeService.DeleteAsync(id);

        return result.Success ? Ok(result) : BadRequest(result);
    }
}
