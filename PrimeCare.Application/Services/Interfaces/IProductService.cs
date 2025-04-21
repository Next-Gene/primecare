using PrimeCare.Application.Dtos;
using PrimeCare.Application.Dtos.Products;

namespace PrimeCare.Application.Services.Interfaces;

public interface IProductService
{
    Task<ProductDto> GetByIdAsync(int id);
    Task<IReadOnlyList<ProductDto>> GetAllAsync();
    Task<ServiceResponse> AddAsync(CreateProductDto entity);
    Task<ServiceResponse> UpdateAsync(CreateProductDto entity);
    Task<ServiceResponse> DeleteAsync(int id);
}
