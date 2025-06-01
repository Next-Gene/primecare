using AutoMapper;
using Microsoft.AspNetCore.Http;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities;
using PrimeCare.Core.Interfaces;
using PrimeCare.Core.Specifications;
using PrimeCare.Shared;
using PrimeCare.Shared.Dtos.Categories;
using PrimeCare.Shared.Dtos.Photos;

/// <summary>
/// Provides services for managing category entities, including CRUD operations and photo management.
/// </summary>
public class CategoryService : ICategoryService
{
    private readonly IGenericRepository<Category> _categoryInterface;
    private readonly IPhotoService _photoService;
    private readonly IMapper _mapper;

    public CategoryService(
        IGenericRepository<Category> categoryInterface,
        IMapper mapper,
        IPhotoService photoService)
    {
        _categoryInterface = categoryInterface;
        _mapper = mapper;
        _photoService = photoService;
    }

    #region Methods

    public async Task<CategoryDto> GetByIdAsync(int id)
    {
        var spec = new CategoryPhotoSepcification(id);
        var category = await _categoryInterface.GetEntityWithSpecification(spec);
        return category == null ? null! : _mapper.Map<CategoryDto>(category);
    }

    public async Task<IReadOnlyList<CategoryDto>> GetAllAsync(string? sort)
    {
        var spec = new CategoryPhotoSepcification(sort);
        var categories = await _categoryInterface.GetAllWithSpecificationAsync(spec);
        return _mapper.Map<IReadOnlyList<CategoryDto>>(categories);
    }

    public async Task<ServiceResponse> AddAsync(CreateCategoryDto entity)
    {
        var mappedData = _mapper.Map<Category>(entity);
        int result = await _categoryInterface.AddAsync(mappedData);

        return result > 0
            ? new ServiceResponse(true, "Category added successfully.")
            : new ServiceResponse(false, "Failed to add category.");
    }

    public async Task<ServiceResponse> UpdateAsync(UpdateCategoryDto entity)
    {
        var category = await _categoryInterface.GetByIdAsync(entity.Id);
        if (category == null)
            return new ServiceResponse(false, "Category Not Found");

        _mapper.Map(entity, category); 
        int result = await _categoryInterface.UpdateAsync(category);

        return result > 0
            ? new ServiceResponse(true, "Category updated successfully.")
            : new ServiceResponse(false, "Failed to update category.");
    }

    public async Task<ServiceResponse> DeleteAsync(int id)
    {
        var category = await _categoryInterface.GetByIdAsync(id);
        if (category == null)
            return new ServiceResponse(false, "Category not found.");

        int result = await _categoryInterface.DeleteAsync(id);

        return result > 0
            ? new ServiceResponse(true, "Category Deleted")
            : new ServiceResponse(false, "Category Not Found or failed to be Deleted");
    }

    public async Task<CategoryPhotoDto?> AddPhotoAsync(int categoryId, IFormFile file)
    {
        var category = await _categoryInterface.GetByIdAsync(categoryId);
        if (category == null)
            return null;

        var result = await _photoService.AddPhotoAsync(file);
        if (result.Error != null)
            return null;

        var categoryPhoto = new CategoryPhoto
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,
            IsMain = !category.CategoryPhotos.Any() 
        };

        category.CategoryPhotos.Add(categoryPhoto);

        int updateResult = await _categoryInterface.UpdateAsync(category);

        if (updateResult > 0)
            return _mapper.Map<CategoryPhotoDto>(categoryPhoto);

        return null;
    }

    #endregion
}