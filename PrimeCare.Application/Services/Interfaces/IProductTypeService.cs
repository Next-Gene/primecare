using PrimeCare.Application.Dtos;
using PrimeCare.Application.Dtos.ProductBrand;

namespace PrimeCare.Application.Services.Interfaces;

public interface IProductTypeService
{
    Task<ProductTypeDto> GetByIdAsync(int id);
    Task<IReadOnlyList<ProductTypeDto>> GetAllAsync();
    Task<ServiceResponse> AddAsync(CreateProductTypeDto entity);
    Task<ServiceResponse> UpdateAsync(ProductTypeDto entity);
    Task<ServiceResponse> DeleteAsync(int id);
}
