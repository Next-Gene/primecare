using AutoMapper;
using PrimeCare.Application.Dtos;
using PrimeCare.Application.Dtos.Categories   ;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities;
using PrimeCare.Core.Interfaces;
using PrimeCare.Core.Specifications;

namespace PrimeCare.Application.Services.Implementations;

/// <summary>
/// Service for managing categories, providing CRUD operations for category entities.
/// </summary>
public class CategoryService : ICategoryService
{
    private readonly IGenericRepository<Category> _categoryInterface;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryService"/> class.
    /// </summary>
    /// <param name="productTypeInterface">The category repository interface used for CRUD operations.</param>
    /// <param name="mapper">The AutoMapper instance for mapping between DTOs and entities.</param>
    public CategoryService(IGenericRepository<Category> categoryInterface, IMapper mapper)
    {
        _categoryInterface = categoryInterface;
        _mapper = mapper;
    }

    #region Methods

    /// <summary>
    /// Retrieves a category type by its identifier asynchronously.
    /// </summary>
    /// <param name="id">The category identifier.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the category data transfer object, or null if not found.</returns>
    public async Task<CategoryDto> GetByIdAsync(int id)
    {
        var category = await _categoryInterface.GetByIdAsync(id);
        return category == null ? null! : _mapper.Map<Category, CategoryDto>(category);
    }





    /// <summary>
    /// Retrieves all categories asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of category data transfer objects.</returns>
    public async Task<IReadOnlyList<CategoryDto>> GetAllAsync()
    {
        var spec = new CategoryPhotoSepcification();
        var categories = await _categoryInterface.GetAllWithSpecificationAsync(spec);
        return _mapper.Map<IReadOnlyList<Category>, IReadOnlyList<CategoryDto>>(categories);
    }

    /// <summary>
    /// Adds a new category asynchronously.
    /// </summary>
    /// <param name="entity">The product type data transfer object (DTO) to be added.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the service response indicating success or failure.</returns>
    public async Task<ServiceResponse> AddAsync(CreateCategoryDto entity)
    {
        var mappedData = _mapper.Map<Category>(entity);
        int result = await _categoryInterface.AddAsync(mappedData);

        return result > 0
            ? new ServiceResponse(true, "Category Added")
            : new ServiceResponse(false, "Category failed to be Added");
    }

    /// <summary>
    /// Updates an existing category asynchronously.
    /// </summary>
    /// <param name="entity">The category data transfer object (DTO) to be updated.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the service response indicating success or failure.</returns>
    public async Task<ServiceResponse> UpdateAsync(UpdateCategoryDto entity)
    {
        var category = await _categoryInterface.GetByIdAsync(entity.Id);
        if (category == null)
        {
            return new ServiceResponse(false, "Category Not Found");
        }

        var mappedData = _mapper.Map(entity, category);
        int result = await _categoryInterface.UpdateAsync(mappedData!);

        return result > 0
            ? new ServiceResponse(true, "Category Updated")
            : new ServiceResponse(false, "Category Type failed to be Updated");
    }

    /// <summary>
    /// Deletes a category by its identifier asynchronously.
    /// </summary>
    /// <param name="id">The category identifier.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the service response indicating success or failure.</returns>
    public async Task<ServiceResponse> DeleteAsync(int id)
    {
        int result = await _categoryInterface.DeleteAsync(id);

        return result > 0
            ? new ServiceResponse(true, "Category Deleted")
            : new ServiceResponse(false, "Category Not Found or failed to be Deleted");
    }

    #endregion
}