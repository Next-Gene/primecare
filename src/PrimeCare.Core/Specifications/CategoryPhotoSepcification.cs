using PrimeCare.Core.Entities;

namespace PrimeCare.Core.Specifications;

/// <summary>
/// Provides specification logic for querying <see cref="Category"/> entities with their associated <see cref="CategoryPhoto"/> entities included.
/// Supports sorting and filtering by category ID or name.
/// </summary>
public class CategoryPhotoSepcification : BaseSpecification<Category>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryPhotoSepcification"/> class
    /// with optional sorting by category name.
    /// </summary>
    /// <param name="sort">
    /// The sorting criteria. If "nameDesc", sorts by name descending; otherwise, sorts by name ascending.
    /// </param>
    public CategoryPhotoSepcification(string? sort)
        : base(null!)
    {
        // Include related category photos
        AddInclude(x => x.CategoryPhotos);

        // Default sorting by name
        AddOrderBy(x => x.Name);

        // Apply sorting based on the provided criteria
        if (!string.IsNullOrEmpty(sort))
        {
            switch (sort)
            {
                case "nameDesc":
                    AddOrderByDescending(p => p.Name);
                    break;
                default:
                    AddOrderBy(p => p.Name);
                    break;
            }
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryPhotoSepcification"/> class
    /// for a specific category by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the category.</param>
    public CategoryPhotoSepcification(int id)
        : base(x => x.Id == id)
    {
        // Include related category photos
        AddInclude(x => x.CategoryPhotos);
    }
}
