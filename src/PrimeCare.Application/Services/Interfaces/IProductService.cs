using PrimeCare.Shared;
using PrimeCare.Shared.Dtos.Products;
using Microsoft.AspNetCore.Http;

namespace PrimeCare.Application.Services.Interfaces;

public interface IProductService
{
    Task<ProductDto> GetByIdAsync(int id);
    Task<IReadOnlyList<ProductDto>> GetAllAsync();
    Task<ServiceResponse> AddAsync(CreateProductDto entity);
    Task<ServiceResponse> UpdateAsync(UpdateProductDto entity);
    Task<ServiceResponse> DeleteAsync(int id);
    Task<ServiceResponse> AddPhotoAsync(int id, IFormFile file);
    Task<ServiceResponse> DeletePhotoAsync(int id, string publicId);
}
