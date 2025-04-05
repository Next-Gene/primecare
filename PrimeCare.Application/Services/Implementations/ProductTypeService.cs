using AutoMapper;
using PrimeCare.Application.Dtos;
using PrimeCare.Application.Dtos.ProductBrand;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities;
using PrimeCare.Core.Interfaces;

namespace PrimeCare.Application.Services.Implementations;

public class ProductTypeService : IProductTypeService
{
    private readonly IGenericRepository<ProductType> _productTypeInterface;
    private readonly IMapper _mapper;

    public ProductTypeService(IGenericRepository<ProductType> productTypeInterface,
        IMapper mapper)
    {
        _productTypeInterface = productTypeInterface;
        _mapper = mapper;
    }

    public async Task<ServiceResponse> AddAsync(CreateProductTypeDto entity)
    {
        var mappedData = _mapper.Map<ProductType>(entity);
        int result = await _productTypeInterface.AddAsync(mappedData);

        return result > 0
            ? new ServiceResponse(true, "Product Type Added")
            : new ServiceResponse(false, "Product Type failed to be Added");
    }

    public async Task<ServiceResponse> DeleteAsync(int id)
    {
        int result = await _productTypeInterface.DeleteAsync(id);

        return result > 0
            ? new ServiceResponse(true, "Product Type Deleted")
            : new ServiceResponse(false, "Product Type Not Found or failed to be Deleted");
    }

    public async Task<ProductTypeDto> GetByIdAsync(int id)
    {
        var product = await _productTypeInterface.GetByIdAsync(id);
        if (product == null) return null!;
        return _mapper.Map<ProductType, ProductTypeDto>(product);
    }

    public async Task<IReadOnlyList<ProductTypeDto>> GetAllAsync()
    {
        var products = await _productTypeInterface.GetAllAsync();
        return _mapper.Map<IReadOnlyList<ProductType>, IReadOnlyList<ProductTypeDto>>(products);
    }

    public async Task<ServiceResponse> UpdateAsync(ProductTypeDto entity)
    {
        var productBrand = await _productTypeInterface.GetByIdAsync(entity.Id);
        var mappedData = _mapper.Map(entity, productBrand);
        int result = await _productTypeInterface.UpdateAsync(mappedData!);

        return result > 0
            ? new ServiceResponse(true, "Product Type Updated")
            : new ServiceResponse(false, "Product Type failed to be Updated");
    }
}
