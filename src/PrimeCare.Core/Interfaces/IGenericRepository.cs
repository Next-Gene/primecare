using PrimeCare.Core.Entities;
using PrimeCare.Core.Specifications;

namespace PrimeCare.Core.Interfaces;

/// <summary>
/// Defines a generic repository interface for performing CRUD operations and queries on entities.
/// </summary>
/// <typeparam name="T">The type of the entity, which must inherit from <see cref="BaseEntity"/>.</typeparam>
public interface IGenericRepository<T> where T : BaseEntity
{
    /// <summary>
    /// Gets an entity by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>The entity if found; otherwise, <c>null</c>.</returns>
    Task<T?> GetByIdAsync(int id);

    /// <summary>
    /// Gets all entities of type <typeparamref name="T"/> asynchronously.
    /// </summary>
    /// <returns>A read-only list of all entities.</returns>
    Task<IReadOnlyList<T>> GetAllAsync();

    /// <summary>
    /// Gets a single entity that matches the specified specification asynchronously.
    /// </summary>
    /// <param name="specification">The specification to apply to the query.</param>
    /// <returns>The entity if found; otherwise, <c>null</c>.</returns>
    Task<T?> GetEntityWithSpecification(ISpecification<T> specification);

    /// <summary>
    /// Gets all entities that match the specified specification asynchronously.
    /// </summary>
    /// <param name="specification">The specification to apply to the query.</param>
    /// <returns>A read-only list of matching entities.</returns>
    Task<IReadOnlyList<T>> GetAllWithSpecificationAsync(ISpecification<T> specification);

    /// <summary>
    /// Adds a new entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>The number of state entries written to the database.</returns>
    Task<int> AddAsync(T entity);

    /// <summary>
    /// Updates an existing entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>The number of state entries written to the database.</returns>
    Task<int> UpdateAsync(T entity);

    /// <summary>
    /// Deletes an entity by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    /// <returns>The number of state entries written to the database.</returns>
    Task<int> DeleteAsync(int id);

    /// <summary>
    /// Saves all changes made in the context to the database asynchronously.
    /// </summary>
    /// <returns><c>true</c> if the changes were saved successfully; otherwise, <c>false</c>.</returns>
    Task<bool> SaveAllAsync();
}
