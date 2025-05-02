using Microsoft.AspNetCore.Http;
using PrimeCare.Shared;
using PrimeCare.Shared.Dtos.Photos;
using PrimeCare.Shared.Dtos.Products;

namespace PrimeCare.Application.Services.Interfaces;

public interface IProductService
{
    Task<ProductDto> GetByIdAsync(int id);
    Task<IReadOnlyList<ProductDto>> GetAllAsync(string? sort, int? brandId, int? categoryId);
    Task<ServiceResponse> AddAsync(CreateProductDto entity);
    Task<ServiceResponse> UpdateAsync(UpdateProductDto entity);
    Task<ServiceResponse> DeleteAsync(int id);
    Task<ProductPhotoDto?> AddPhotoAsync(int productId, IFormFile file);
}
