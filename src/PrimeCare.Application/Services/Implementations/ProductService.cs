using AutoMapper;
using Microsoft.AspNetCore.Http;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities;
using PrimeCare.Core.Interfaces;
using PrimeCare.Core.Specifications;
using PrimeCare.Shared;
using PrimeCare.Shared.Dtos.Products;

namespace PrimeCare.Application.Services.Implementations;

/// <summary>
/// Provides services for managing products.
/// </summary>
public class ProductService : IProductService
{
    private readonly IGenericRepository<Product> _productInterface;
    private readonly IGenericRepository<ProductPhotos> _productPhotoInterface;
    private readonly IMapper _mapper;
    private readonly IPhotoService _photoService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductService"/> class.
    /// </summary>
    /// <param name="productInterface">The product repository interface.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    public ProductService(IGenericRepository<Product> productInterface,
                         IMapper mapper,
                         IPhotoService photoService,
                         IGenericRepository<ProductPhotos> productPhotoInterface)
    {
        _productInterface = productInterface;
        _mapper = mapper;
        _photoService = photoService;
        _productPhotoInterface = productPhotoInterface;
    }

    #region Methods

    /// <summary>
    /// Retrieves a product by its identifier asynchronously.
    /// </summary>
    /// <param name="id">The product identifier.</param>
    /// <returns>A task that represents the asynchronous operation, with the product DTO as result.</returns>
    public async Task<ProductDto> GetByIdAsync(int id)
    {
        var spec = new ProductsWithBrandsAndCategoriesAndPhotosSpecification(id);
        var product = await _productInterface.GetEntityWithSpecification(spec);
        return product == null ? null! : _mapper.Map<ProductDto>(product);
    }

    /// <summary>
    /// Retrieves all products asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, with a list of product DTOs as result.</returns>
    public async Task<IReadOnlyList<ProductDto>> GetAllAsync()
    {
        var spec = new ProductsWithBrandsAndCategoriesAndPhotosSpecification();
        var products = await _productInterface.GetAllWithSpecificationAsync(spec);
        return _mapper.Map<IReadOnlyList<ProductDto>>(products);
    }

    /// <summary>
    /// Adds a new product asynchronously.
    /// </summary>
    /// <param name="entity">The product data transfer object (DTO) to be added.</param>
    /// <returns>A task that represents the asynchronous operation. The result contains a service response indicating success or failure.</returns>
    public async Task<ServiceResponse> AddAsync(CreateProductDto entity)
    {
        var mappedData = _mapper.Map<Product>(entity);
        int result = await _productInterface.AddAsync(mappedData);

        return result > 0
            ? new ServiceResponse(true, "Product Added")
            : new ServiceResponse(false, "Product failed to be Added");
    }

    /// <summary>
    /// Updates an existing product asynchronously.
    /// </summary>
    /// <param name="entity">The product data transfer object (DTO) to be updated.</param>
    /// <returns>A task that represents the asynchronous operation. The result contains a service response indicating success or failure.</returns>
    public async Task<ServiceResponse> UpdateAsync(UpdateProductDto entity)
    {
        var product = await _productInterface.GetByIdAsync(entity.Id);
        var mappedData = _mapper.Map(entity, product);
        int result = await _productInterface.UpdateAsync(mappedData!);

        return result > 0
            ? new ServiceResponse(true, "Product Updated")
            : new ServiceResponse(false, "Product failed to be Updated");
    }

    /// <summary>
    /// Deletes a product by its identifier asynchronously.
    /// </summary>
    /// <param name="id">The product identifier.</param>
    /// <returns>A task that represents the asynchronous operation. The result contains a service response indicating success or failure.</returns>
    public async Task<ServiceResponse> DeleteAsync(int id)
    {
        int result = await _productInterface.DeleteAsync(id);

        return result > 0
            ? new ServiceResponse(true, "Product Deleted")
            : new ServiceResponse(false, "Product Not Found or failed to be Deleted");
    }

    public async Task<ServiceResponse> AddPhotoAsync(int id, IFormFile file)
    {
        var product = await GetByIdAsync(id);
        if (product == null)
        {
            return new ServiceResponse(false, "Product Not Found");
        }

        var result = await _photoService.AddPhotoAsync(file);
        if (result.Error != null)
        {
            return new ServiceResponse(false, result.Error.Message.ToString());
        }

        var photo = new ProductPhotos
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,
            ProductId = id,
            IsMain = product.ProductPhotos.Count == 0 // Set as main if it's the first photo
        };

        await _productPhotoInterface.AddAsync(photo);
        return new ServiceResponse(true, "Photo added successfully");
    }

    public async Task<ServiceResponse> DeletePhotoAsync(int id, string publicId)
    {
        var product = await GetByIdAsync(id);
        if (product == null)
        {
            return new ServiceResponse(false, "Product Not Found");
        }

        var photo = product.ProductPhotos.FirstOrDefault(p => p.PublicId == publicId);
        if (photo == null)
        {
            return new ServiceResponse(false, "Photo Not Found");
        }

        var result = await _photoService.DeletePhotoAsync(publicId);
        if (result.Error != null)
        {
            return new ServiceResponse(false, result.Error.Message.ToString());
        }

        await _productPhotoInterface.DeleteAsync(photo.Id);
        return new ServiceResponse(true, "Photo deleted successfully");
    }
    #endregion
}