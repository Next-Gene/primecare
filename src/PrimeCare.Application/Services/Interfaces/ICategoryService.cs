using PrimeCare.Shared;
using PrimeCare.Shared.Dtos.Categories;

namespace PrimeCare.Application.Services.Interfaces;

public interface ICategoryService
{
    Task<CategoryDto> GetByIdAsync(int id);
    Task<IReadOnlyList<CategoryDto>> GetAllAsync();
    Task<ServiceResponse> AddAsync(CreateCategoryDto entity);
    Task<ServiceResponse> UpdateAsync(UpdateCategoryDto entity);
    Task<ServiceResponse> DeleteAsync(int id);
    //Task<ServiceResponse> AddPhotoAsync(int id, IFormFile file);


}
