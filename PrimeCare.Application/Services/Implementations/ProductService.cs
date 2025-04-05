using AutoMapper;
using PrimeCare.Application.Dtos;
using PrimeCare.Application.Dtos.Product;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities;
using PrimeCare.Core.Interfaces;
using PrimeCare.Core.Specifications;

namespace PrimeCare.Application.Services.Implementations;

public class ProductService : IProductService
{
    private readonly IGenericRepository<Product> _productInterface;
    private readonly IMapper _mapper;

    public ProductService(IGenericRepository<Product> productInterface, IMapper mapper)
    {
        _productInterface = productInterface;
        _mapper = mapper;
    }

    public async Task<ServiceResponse> AddAsync(CreateProductDto entity)
    {
        var mappedData = _mapper.Map<Product>(entity);
        int result = await _productInterface.AddAsync(mappedData);

        return result > 0
            ? new ServiceResponse(true, "Product Added")
            : new ServiceResponse(false, "Product failed to be Added");
    }

    public async Task<ServiceResponse> DeleteAsync(int id)
    {
        int result = await _productInterface.DeleteAsync(id);

        return result > 0
            ? new ServiceResponse(true, "Product Deleted")
            : new ServiceResponse(false, "Product Not Found or failed to be Deleted");
    }


    public async Task<ProductDto> GetByIdAsync(int id)
    {
        var spec = new ProductsWithTypesAndBrandsSpecification(id);
        var product = await _productInterface.GetEntityWithSpecification(spec);
        if (product == null) return null!;
        return _mapper.Map<Product, ProductDto>(product);
    }

    public async Task<IReadOnlyList<ProductDto>> GetAllAsync()
    {
        var spec = new ProductsWithTypesAndBrandsSpecification();
        var products = await _productInterface.GetAllWithSpecificationAsync(spec);
        return _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDto>>(products);
    }

    public async Task<ServiceResponse> UpdateAsync(CreateProductDto entity)
    {
        var mappedData = _mapper.Map<Product>(entity);
        int result = await _productInterface.UpdateAsync(mappedData);

        return result > 0
            ? new ServiceResponse(true, "Product Updated")
            : new ServiceResponse(false, "Product failed to be Updated");
    }
}
