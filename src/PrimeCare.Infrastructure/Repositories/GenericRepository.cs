using Microsoft.EntityFrameworkCore;
using PrimeCare.Core.Entities;
using PrimeCare.Core.Interfaces;
using PrimeCare.Core.Specifications;
using PrimeCare.Infrastructure.Data;
using PrimeCare.Shared.Exceptions;

namespace PrimeCare.Infrastructure.Repositories;

/// <summary>
/// Provides a generic repository implementation for managing entities in the database.
/// Supports CRUD operations and querying with specifications.
/// </summary>
/// <typeparam name="T">The type of the entity, which must inherit from <see cref="BaseEntity"/>.</typeparam>
public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly PrimeCareContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenericRepository{T}"/> class.
    /// </summary>
    /// <param name="context">The database context to use for data operations.</param>
    public GenericRepository(PrimeCareContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets an entity by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>The entity if found; otherwise, <c>null</c>.</returns>
    public async Task<T?> GetByIdAsync(int id)
        => await _context.Set<T>().FindAsync(id);

    /// <summary>
    /// Gets all entities of type <typeparamref name="T"/> asynchronously.
    /// </summary>
    /// <returns>A read-only list of all entities.</returns>
    public async Task<IReadOnlyList<T>> GetAllAsync()
        => await _context.Set<T>().AsNoTracking().ToListAsync();

    /// <summary>
    /// Gets a single entity that matches the specified specification asynchronously.
    /// </summary>
    /// <param name="specification">The specification to apply to the query.</param>
    /// <returns>The entity if found; otherwise, <c>null</c>.</returns>
    public async Task<T?> GetEntityWithSpecification(ISpecification<T> specification)
        => await ApplySpecification(specification).FirstOrDefaultAsync();

    /// <summary>
    /// Gets all entities that match the specified specification asynchronously.
    /// </summary>
    /// <param name="specification">The specification to apply to the query.</param>
    /// <returns>A read-only list of matching entities.</returns>
    public async Task<IReadOnlyList<T>> GetAllWithSpecificationAsync(ISpecification<T> specification)
        => await ApplySpecification(specification).AsNoTracking().ToListAsync();

    /// <summary>
    /// Adds a new entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>The number of state entries written to the database.</returns>
    public async Task<int> AddAsync(T entity)
    {
        _context.Set<T>().Add(entity);
        return await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>The number of state entries written to the database.</returns>
    public async Task<int> UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        return await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes an entity by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    /// <returns>The number of state entries written to the database.</returns>
    /// <exception cref="ItemNotFoundException">Thrown when the entity with the specified identifier is not found.</exception>
    public async Task<int> DeleteAsync(int id)
    {
        var entity = await _context.Set<T>().FindAsync(id)
            ?? throw new ItemNotFoundException($"Item with {id} is not found");
        _context.Set<T>().Remove(entity);
        return await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Saves all changes made in the context to the database asynchronously.
    /// </summary>
    /// <returns><c>true</c> if the changes were saved successfully; otherwise, <c>false</c>.</returns>
    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    /// <summary>
    /// Applies the given specification to the query.
    /// </summary>
    /// <param name="specification">The specification to apply.</param>
    /// <returns>The query with the specification applied.</returns>
    private IQueryable<T> ApplySpecification(ISpecification<T> specification)
        => SpecificationEvaluator<T>
        .GetQuery(_context.Set<T>().AsQueryable(), specification);
}
