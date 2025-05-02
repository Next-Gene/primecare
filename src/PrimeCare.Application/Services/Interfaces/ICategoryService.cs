using Microsoft.AspNetCore.Http;
using PrimeCare.Shared;
using PrimeCare.Shared.Dtos.Categories;
using PrimeCare.Shared.Dtos.Photos;

namespace PrimeCare.Application.Services.Interfaces;

public interface ICategoryService
{
    Task<CategoryDto> GetByIdAsync(int id);
    Task<IReadOnlyList<CategoryDto>> GetAllAsync(string? sort);
    Task<ServiceResponse> AddAsync(CreateCategoryDto entity);
    Task<ServiceResponse> UpdateAsync(UpdateCategoryDto entity);
    Task<ServiceResponse> DeleteAsync(int id);
    Task<CategoryPhotoDto?> AddPhotoAsync(int categoryId, IFormFile file);

}
