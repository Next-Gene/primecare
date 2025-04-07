using AutoMapper;
using PrimeCare.Application.Dtos;
using PrimeCare.Application.Dtos.ProductBrand;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities;
using PrimeCare.Core.Interfaces;

namespace PrimeCare.Application.Services.Implementations;

/// <summary>
/// Service for managing product brands.
/// </summary>
public class ProductBrandService : IProductBrandService
{
    private readonly IGenericRepository<ProductBrand> _productBrandInterface;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductBrandService"/> class.
    /// </summary>
    /// <param name="productBrandInterface">The product brand repository interface.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    public ProductBrandService(IGenericRepository<ProductBrand> productBrandInterface,
        IMapper mapper)
    {
        _productBrandInterface = productBrandInterface;
        _mapper = mapper;
    }

    /// <summary>
    /// Adds a new product brand asynchronously.
    /// </summary>
    /// <param name="entity">The product brand data transfer object.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the service response.</returns>
    public async Task<ServiceResponse> AddAsync(CreateProductBrandDto entity)
    {
        var mappedData = _mapper.Map<ProductBrand>(entity);
        int result = await _productBrandInterface.AddAsync(mappedData);

        return result > 0
            ? new ServiceResponse(true, "Product Brand Added")
            : new ServiceResponse(false, "Product Brand failed to be Added");
    }

    /// <summary>
    /// Deletes a product brand by its identifier asynchronously.
    /// </summary>
    /// <param name="id">The product brand identifier.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the service response.</returns>
    public async Task<ServiceResponse> DeleteAsync(int id)
    {
        int result = await _productBrandInterface.DeleteAsync(id);

        return result > 0
            ? new ServiceResponse(true, "Product Brand Deleted")
            : new ServiceResponse(false, "Product Brand Not Found or failed to be Deleted");
    }

    /// <summary>
    /// Gets a product brand by its identifier asynchronously.
    /// </summary>
    /// <param name="id">The product brand identifier.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the product brand data transfer object.</returns>
    public async Task<ProductBrandDto> GetByIdAsync(int id)
    {
        var product = await _productBrandInterface.GetByIdAsync(id);
        if (product == null) return null!;
        return _mapper.Map<ProductBrand, ProductBrandDto>(product);
    }

    /// <summary>
    /// Gets all product brands asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of product brand data transfer objects.</returns>
    public async Task<IReadOnlyList<ProductBrandDto>> GetAllAsync()
    {
        var products = await _productBrandInterface.GetAllAsync();
        return _mapper.Map<IReadOnlyList<ProductBrand>, IReadOnlyList<ProductBrandDto>>(products);
    }

    /// <summary>
    /// Updates a product brand asynchronously.
    /// </summary>
    /// <param name="entity">The product brand data transfer object.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the service response.</returns>
    public async Task<ServiceResponse> UpdateAsync(ProductBrandDto entity)
    {
        var productBrand = await _productBrandInterface.GetByIdAsync(entity.Id);
        var mappedData = _mapper.Map(entity, productBrand);
        int result = await _productBrandInterface.UpdateAsync(mappedData!);

        return result > 0
            ? new ServiceResponse(true, "Product Brand Updated")
            : new ServiceResponse(false, "Product Brand failed to be Updated");
    }
}