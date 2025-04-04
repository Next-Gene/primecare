using PrimeCare.Application.Dtos;
using PrimeCare.Application.Dtos.ProductBrand;

namespace PrimeCare.Application.Services.Interfaces;

public interface IProductBrandService
{
    Task<ProductBrandDto> GetByIdAsync(int id);
    Task<IReadOnlyList<ProductBrandDto>> GetAllAsync();
    Task<ServiceResponse> AddAsync(CreateProductBrandDto entity);
    Task<ServiceResponse> UpdateAsync(ProductBrandDto entity);
    Task<ServiceResponse> DeleteAsync(int id);
}
