using Microsoft.AspNetCore.Mvc;
using PrimeCare.Application.Dtos.Products;
using PrimeCare.Application.Errors;
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

   

