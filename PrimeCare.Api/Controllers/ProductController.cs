using Microsoft.AspNetCore.Mvc;
using PrimeCare.Core.Entities;
using PrimeCare.Core.Interfaces;

namespace PrimeCare.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    public ProductController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetProducts()
    {
        //var products = await _productRepository.GetProductsAsync();
        var p = new List<Product>
        {
            new Product{Id=1,Name="Hmada",Description="asfdsfsd"}
        };
        return Ok(p);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
        => await _productRepository.GetProductByIdAsync(id);

}
