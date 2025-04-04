using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PrimeCare.Core.Entities;
using PrimeCare.Core.Interfaces;
using PrimeCare.Core.Specifications;

namespace PrimeCare.Api.Controllers;

public class ProductController : BaseApiController
{
    private readonly IGenericRepository<Product> _productRepo;
    private readonly IGenericRepository<ProductBrand> _productBrandRepo;
    private readonly IGenericRepository<ProductType> _productTypeRepo;
    private readonly IMapper _mapper;

    public ProductController(IGenericRepository<Product> productRepo,
        IGenericRepository<ProductBrand> productBrandRepo,
        IGenericRepository<ProductType> productTypeRepo, IMapper mapper)
    {
        _productRepo = productRepo;
        _productBrandRepo = productBrandRepo;
        _productTypeRepo = productTypeRepo;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetProducts()
    {
        var specification = new ProductsWithTypesAndBrandsSpecification();
        var products = await _productRepo.ListAsync(specification);
        return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDto>>(products));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var specification = new ProductsWithTypesAndBrandsSpecification(id);
        var product = await _productRepo.GetEntityWithSpecification(specification);
        return _mapper.Map<Product, ProductDto>(product!);
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        => Ok(await _productBrandRepo.ListAllAsync());

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        => Ok(await _productTypeRepo.ListAllAsync());

}
