using AutoMapper;
using Microsoft.AspNetCore.Http;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities;
using PrimeCare.Core.Interfaces;
using PrimeCare.Core.Specifications;
using PrimeCare.Shared;
using PrimeCare.Shared.Dtos.Photos;
using PrimeCare.Shared.Dtos.Products;

namespace PrimeCare.Application.Services.Implementations;

/// <summary>
/// Provides services for managing products, including CRUD operations and photo management.
/// </summary>
public class ProductService : IProductService
{
    private readonly IGenericRepository<Product> _productInterface;
    private readonly IPhotoService _photoService;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductService"/> class.
    /// </summary>
    /// <param name="productInterface">The product repository interface for data access.</param>
    /// <param name="mapper">The AutoMapper instance for mapping entities and DTOs.</param>
    /// <param name="photoService">The photo service for managing product photos.</param>
    public ProductService(IGenericRepository<Product> productInterface,
        IMapper mapper, IPhotoService photoService)
    {
        _productInterface = productInterface;
        _photoService = photoService;
        _mapper = mapper;
    }

    #region Methods

    /// <summary>
    /// Retrieves a product by its identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. 
    /// The task result contains the product DTO if found; otherwise, <c>null</c>.
    /// </returns>
    public async Task<ProductDto> GetByIdAsync(int id)
    {
        var spec = new ProductsWithBrandsAndCategoriesAndPhotosSpecification(id);
        var product = await _productInterface.GetEntityWithSpecification(spec);
        return product == null ? null! : _mapper.Map<ProductDto>(product);
    }

    /// <summary>
    /// Retrieves all products asynchronously with optional sorting and filtering.
    /// </summary>
    /// <param name="sort">The sorting criteria (e.g., "priceAsc", "priceDesc").</param>
    /// <param name="brandId">The brand ID to filter by (optional).</param>
    /// <param name="categoryId">The category ID to filter by (optional).</param>
    /// <returns>
    /// A task that represents the asynchronous operation. 
    /// The task result contains a list of product DTOs.
    /// </returns>
    public async Task<IReadOnlyList<ProductDto>> GetAllAsync(
        string? sort, int? brandId, int? categoryId)
    {
        var spec = new ProductsWithBrandsAndCategoriesAndPhotosSpecification(sort, brandId, categoryId);
        var products = await _productInterface.GetAllWithSpecificationAsync(spec);
        return _mapper.Map<IReadOnlyList<ProductDto>>(products);
    }

    /// <summary>
    /// Adds a new product asynchronously.
    /// </summary>
    /// <param name="entity">The product data transfer object (DTO) to be added.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. 
    /// The task result contains a <see cref="ServiceResponse"/> indicating success or failure.
    /// </returns>
    public async Task<ServiceResponse> AddAsync(CreateProductDto entity)
    {
        var mappedData = _mapper.Map<Product>(entity);
        int result = await _productInterface.AddAsync(mappedData);

        return result > 0
            ? new ServiceResponse(true, "Product added successfully.")
            : new ServiceResponse(false, "Failed to add product.");
    }

    /// <summary>
    /// Updates an existing product asynchronously.
    /// </summary>
    /// <param name="entity">The product data transfer object (DTO) containing updated details.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. 
    /// The task result contains a <see cref="ServiceResponse"/> indicating success or failure.
    /// </returns>
    public async Task<ServiceResponse> UpdateAsync(UpdateProductDto entity)
    {
        var product = await _productInterface.GetByIdAsync(entity.Id);
        if (product == null)
            return new ServiceResponse(false, "Product not found.");

        var mappedData = _mapper.Map(entity, product);
        int result = await _productInterface.UpdateAsync(mappedData);

        return result > 0
            ? new ServiceResponse(true, "Product updated successfully.")
            : new ServiceResponse(false, "Failed to update product.");
    }

    /// <summary>
    /// Deletes a product by its identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the product to delete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. 
    /// The task result contains a <see cref="ServiceResponse"/> indicating success or failure.
    /// </returns>
    public async Task<ServiceResponse> DeleteAsync(int id)
    {
        var product = await _productInterface.GetByIdAsync(id);
        if (product == null)
            return new ServiceResponse(false, "Product not found.");

        int result = await _productInterface.DeleteAsync(id);

        return result > 0
            ? new ServiceResponse(true, "Product deleted successfully.")
            : new ServiceResponse(false, "Failed to delete product.");
    }

    /// <summary>
    /// Adds a photo to a specific product asynchronously.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <param name="file">The photo file to be added.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. 
    /// The task result contains the added photo DTO if successful; otherwise, <c>null</c>.
    /// </returns>
    public async Task<ProductPhotoDto?> AddPhotoAsync(int productId, IFormFile file)
    {
        var product = await _productInterface.GetByIdAsync(productId);
        if (product == null)
            return null;

        var result = await _photoService.AddPhotoAsync(file);
        if (result.Error != null)
            return null;

        var productPhoto = new ProductPhoto
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,
            IsMain = product.ProductPhotos.Count == 0
        };

        product.ProductPhotos.Add(productPhoto);
        if (await _productInterface.SaveAllAsync())
            return _mapper.Map<ProductPhotoDto>(productPhoto);

        return null;
    }

    #endregion
}
