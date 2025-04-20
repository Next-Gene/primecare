using PrimeCare.Application.Dtos;
using PrimeCare.Application.Dtos.Category;


namespace PrimeCare.Application.Services.Interfaces;

public interface ICategoryService
{
    Task<CategoryDto> GetByIdAsync(int id);
    Task<IReadOnlyList<CategoryDto>> GetAllAsync();
    Task<ServiceResponse> AddAsync(CreateCategoryDto entity);
    Task<ServiceResponse> UpdateAsync(UpdateCategoryDto entity);
    Task<ServiceResponse> DeleteAsync(int id);
}
