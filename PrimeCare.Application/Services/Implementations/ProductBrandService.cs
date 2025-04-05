using AutoMapper;
using PrimeCare.Application.Dtos;
using PrimeCare.Application.Dtos.ProductBrand;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities;
using PrimeCare.Core.Interfaces;

namespace PrimeCare.Application.Services.Implementations;

public class ProductBrandService : IProductBrandService
{
    private readonly IGenericRepository<ProductBrand> _productBrandInterface;
    private readonly IMapper _mapper;

    public ProductBrandService(IGenericRepository<ProductBrand> productBrandInterface,
        IMapper mapper)
    {
        _productBrandInterface = productBrandInterface;
        _mapper = mapper;
    }

    public async Task<ServiceResponse> AddAsync(CreateProductBrandDto entity)
    {
        var mappedData = _mapper.Map<ProductBrand>(entity);
        int result = await _productBrandInterface.AddAsync(mappedData);

        return result > 0
            ? new ServiceResponse(true, "Product Brand Added")
            : new ServiceResponse(false, "Product Brand failed to be Added");
    }

    public async Task<ServiceResponse> DeleteAsync(int id)
    {
        int result = await _productBrandInterface.DeleteAsync(id);

        return result > 0
            ? new ServiceResponse(true, "Product Brand Deleted")
            : new ServiceResponse(false, "Product Brand Not Found or failed to be Deleted");
    }

    public async Task<ProductBrandDto> GetByIdAsync(int id)
    {
        var product = await _productBrandInterface.GetByIdAsync(id);
        if (product == null) return new ProductBrandDto();
        return _mapper.Map<ProductBrand, ProductBrandDto>(product);
    }

    public async Task<IReadOnlyList<ProductBrandDto>> GetAllAsync()
    {
        var products = await _productBrandInterface.GetAllAsync();
        if (!products.Any()) return [];
        return _mapper.Map<IReadOnlyList<ProductBrand>, IReadOnlyList<ProductBrandDto>>(products);
    }

    public async Task<ServiceResponse> UpdateAsync(ProductBrandDto entity)
    {
        var productBrand = await _productBrandInterface.GetByIdAsync(entity.Id);
        var mappedData = _mapper.Map(entity, productBrand);
        int result = await _productBrandInterface.UpdateAsync(mappedData);

        return result > 0
            ? new ServiceResponse(true, "Product Brand Updated")
            : new ServiceResponse(false, "Product Brand failed to be Updated");
    }
}
