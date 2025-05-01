using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities;
using PrimeCare.Core.Interfaces;
using PrimeCare.Shared.Dtos.Photos;
using PrimeCare.Shared.Dtos.Products;
using PrimeCare.Shared.Errors;

namespace PrimeCare.Api.Controllers;

public class ProductController : BaseApiController
{
    private readonly IProductService _productService;
    private readonly IPhotoService _photoService;
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IMapper _mapper;

    public ProductController(IProductService productService,
        IPhotoService photoService, IMapper mapper,
        IGenericRepository<Product> productRepository)
    {
        _productService = productService;
        _photoService = photoService;
        _mapper = mapper;
        _productRepository = productRepository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetProducts(string? sort)
    {
        var products = await _productService.GetAllAsync(sort);
        return products.Any() ? Ok(products) : NotFound(products);
    }

    [HttpGet("{id}", Name = "GetProduct")]
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

    [HttpPost("add-photo/{id}")]
    public async Task<ActionResult<ProductPhotoDto>> AddPhoto(int id, IFormFile file)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            return NotFound($"Product with id {id} not found");

        var result = await _photoService.AddPhotoAsync(file);

        if (result.Error != null) return BadRequest(result.Error.Message);

        var productPhoto = new ProductPhoto
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        if (product!.ProductPhotos.Count == 0)
        {
            productPhoto.IsMain = true;
        }

        product.ProductPhotos.Add(productPhoto);
        if (await _productRepository.SaveAllAsync())
            return CreatedAtRoute("GetCategory", new { id = product.Id }, _mapper.Map<ProductPhotoDto>(productPhoto));
        return BadRequest("Problem Adding Photo");
    }

    //[HttpDelete("photo")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    //public async Task<IActionResult> DeletePhoto(int productId, string publicId)
    //{
    //}
}



