using AutoMapper;
using PrimeCare.Application.Dtos;
using PrimeCare.Application.Dtos.Product;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities;
using PrimeCare.Core.Interfaces;
using PrimeCare.Core.Specifications;

namespace PrimeCare.Application.Services.Implementations;

/// <summary>
/// Provides services for managing products.
/// </summary>
public class ProductService : IProductService
{
    private readonly IGenericRepository<Product> _productInterface;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductService"/> class.
    /// </summary>
    /// <param name="productInterface">The product repository interface.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    public ProductService(IGenericRepository<Product> productInterface, IMapper mapper)
    {
        _productInterface = productInterface;
        _mapper = mapper;
    }

    #region Methods

    /// <summary>
    /// Retrieves a product by its identifier asynchronously.
    /// </summary>
    /// <param name="id">The product identifier.</param>
    /// <returns>A task that represents the asynchronous operation, with the product DTO as result.</returns>
    public async Task<ProductDto> GetByIdAsync(int id)
    {
        var spec = new ProductsWithTypesAndBrandsSpecification(id);
        var product = await _productInterface.GetEntityWithSpecification(spec);
        return product == null ? null! : _mapper.Map<ProductDto>(product);
    }

    /// <summary>
    /// Retrieves all products asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, with a list of product DTOs as result.</returns>
    public async Task<IReadOnlyList<ProductDto>> GetAllAsync()
    {
        var spec = new ProductsWithTypesAndBrandsSpecification();
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
    public async Task<ServiceResponse> UpdateAsync(CreateProductDto entity)
    {
        var mappedData = _mapper.Map<Product>(entity);
        int result = await _productInterface.UpdateAsync(mappedData);

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

    #endregion
}