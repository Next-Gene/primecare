using Microsoft.AspNetCore.Mvc;
using PrimeCare.Application.Services.Interfaces;

namespace PrimeCare.Api.Controllers;

public class ProductController : BaseApiController
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var data = await _productService.ListAsync();
        return data.Any() ? Ok(data) : NotFound(data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var data = await _productService.GetByIdAsync(id);
        return data != null ? Ok(data) : NotFound(data);
    }

    //[HttpGet("brands")]
    //public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
    //    => Ok(await _productBrandRepo.ListAllAsync());

    //[HttpGet("types")]
    //public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
    //    => Ok(await _productTypeRepo.ListAllAsync());

}
