using PrimeCare.Application.Dtos;
using PrimeCare.Application.Dtos.Product;
using PrimeCare.Core.Entities;
using PrimeCare.Core.Specifications;

namespace PrimeCare.Application.Services.Interfaces;

public interface IProductService
{
    Task<ProductDto> GetByIdAsync(int id);
    Task<IReadOnlyList<ProductDto>> ListAllAsync();
    Task<ProductDto> GetEntityWithSpecification(ISpecification<Product> specification);
    Task<IReadOnlyList<ProductDto>> ListAsync(ISpecification<Product> specification);
    Task<ServiceResponse> AddAsync(CreateProductDto entity);
    Task<ServiceResponse> UpdateAsync(CreateProductDto entity);
    Task<ServiceResponse> DeleteAsync(int id);
}
