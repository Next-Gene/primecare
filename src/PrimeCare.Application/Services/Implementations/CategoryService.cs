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

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryService"/> class.
    /// </summary>
    /// <param name="categoryInterface">Generic repository for category entities.</param>
    /// <param name="mapper">The AutoMapper instance used for object-object mapping.</param>
    /// <param name="photoService">Service for handling photo uploads and deletions.</param>
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

    /// <summary>
    /// Retrieves a category by its ID, including associated photos.
    /// </summary>
    /// <param name="id">The unique identifier of the category.</param>
    /// <returns>The corresponding <see cref="CategoryDto"/>, or null if not found.</returns>
    public async Task<CategoryDto> GetByIdAsync(int id)
    {
        var spec = new CategoryPhotoSepcification(id);
        var category = await _categoryInterface.GetEntityWithSpecification(spec);
        return category == null ? null! : _mapper.Map<CategoryDto>(category);
    }

    /// <summary>
    /// Retrieves all categories with optional sorting and includes associated photos.
    /// </summary>
    /// <param name="sort">An optional sort parameter (e.g., name or date).</param>
    /// <returns>A list of <see cref="CategoryDto"/> representing the categories.</returns>
    public async Task<IReadOnlyList<CategoryDto>> GetAllAsync(string? sort)
    {
        var spec = new CategoryPhotoSepcification(sort);
        var categories = await _categoryInterface.GetAllWithSpecificationAsync(spec);
        return _mapper.Map<IReadOnlyList<CategoryDto>>(categories);
    }

    /// <summary>
    /// Adds a new category.
    /// </summary>
    /// <param name="entity">The <see cref="CreateCategoryDto"/> containing category details.</param>
    /// <returns>A <see cref="ServiceResponse"/> indicating success or failure.</returns>
    public async Task<ServiceResponse> AddAsync(CreateCategoryDto entity)
    {
        var mappedData = _mapper.Map<Category>(entity);
        int result = await _categoryInterface.AddAsync(mappedData);

        return result > 0
            ? new ServiceResponse(true, "Category added successfully.")
            : new ServiceResponse(false, "Failed to add category.");
    }

    /// <summary>
    /// Updates an existing category.
    /// </summary>
    /// <param name="entity">The <see cref="UpdateCategoryDto"/> containing updated details.</param>
    /// <returns>A <see cref="ServiceResponse"/> indicating success or failure.</returns>
    public async Task<ServiceResponse> UpdateAsync(UpdateCategoryDto entity)
    {
        var category = await _categoryInterface.GetByIdAsync(entity.Id);
        if (category == null)
            return new ServiceResponse(false, "Category Not Found");

        var mappedData = _mapper.Map(entity, category);
        int result = await _categoryInterface.UpdateAsync(mappedData);

        return result > 0
            ? new ServiceResponse(true, "Category updated successfully.")
            : new ServiceResponse(false, "Failed to update category.");
    }

    /// <summary>
    /// Deletes a category by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the category.</param>
    /// <returns>A <see cref="ServiceResponse"/> indicating success or failure.</returns>
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

    /// <summary>
    /// Adds a photo to the specified category.
    /// </summary>
    /// <param name="categoryId">The ID of the category to which the photo will be added.</param>
    /// <param name="file">The photo file to upload.</param>
    /// <returns>The uploaded <see cref="CategoryPhotoDto"/> if successful; otherwise, null.</returns>
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
            IsMain = category.CategoryPhotos.Count == 0
        };

        category.CategoryPhotos.Add(categoryPhoto);
        if (await _categoryInterface.SaveAllAsync())
            return _mapper.Map<CategoryPhotoDto>(categoryPhoto);

        return null;
    }

    #endregion
}
