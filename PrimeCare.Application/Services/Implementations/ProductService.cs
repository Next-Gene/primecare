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
    private readonly IGenericRepository<Product> _productRepo;
    private readonly IMapper _mapper;

    public ProductService(IGenericRepository<Product> productRepo, IMapper mapper)
    {
        _productRepo = productRepo;
        _mapper = mapper;
    }

    public async Task<ServiceResponse> AddAsync(CreateProductDto entity)
    {
        int result = await
    }

    public Task<ServiceResponse> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ProductDto> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ProductDto> GetEntityWithSpecification(ISpecification<Product> specification)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<ProductDto>> ListAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<ProductDto>> ListAsync(ISpecification<Product> specification)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResponse> UpdateAsync(CreateProductDto entity)
    {
        throw new NotImplementedException();
    }
}
