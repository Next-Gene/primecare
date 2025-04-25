using Microsoft.EntityFrameworkCore;
using PrimeCare.Core.Entities;
using PrimeCare.Core.Interfaces;
using PrimeCare.Core.Specifications;
using PrimeCare.Infrastructure.Data;
using PrimeCare.Shared.Exceptions;

namespace PrimeCare.Infrastructure.Repositories;

/// <summary>
/// Generic repository for managing entities.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly PrimeCareContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenericRepository{T}"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public GenericRepository(PrimeCareContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets an entity by its identifier.
    /// </summary>
    /// <param name="id">The entity identifier.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity.</returns>
    public async Task<T?> GetByIdAsync(int id)
        => await _context.Set<T>().FindAsync(id);

    /// <summary>
    /// Gets all entities.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of entities.</returns>
    public async Task<IReadOnlyList<T>> GetAllAsync()
        => await _context.Set<T>().AsNoTracking().ToListAsync();

    /// <summary>
    /// Gets an entity that matches the given specification.
    /// </summary>
    /// <param name="specification">The specification to apply.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity.</returns>
    public async Task<T?> GetEntityWithSpecification(ISpecification<T> specification)
        => await ApplySpecification(specification).FirstOrDefaultAsync();

    /// <summary>
    /// Gets a list of entities that match the given specification.
    /// </summary>
    /// <param name="specification">The specification to apply.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of entities.</returns>
    public async Task<IReadOnlyList<T>> GetAllWithSpecificationAsync(ISpecification<T> specification)
        => await ApplySpecification(specification).AsNoTracking().ToListAsync();

    /// <summary>
    /// Adds a new entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of state entries written to the database.</returns>
    public async Task<int> AddAsync(T entity)
    {
        _context.Set<T>().Add(entity);
        return await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing entity in the repository.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of state entries written to the database.</returns>
    public async Task<int> UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        return await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes an entity by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity to delete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of state entries written to the database.</returns>
    /// <exception cref="ItemNotFoundException">Thrown when the entity with the specified identifier is not found.</exception>
    public async Task<int> DeleteAsync(int id)
    {
        var entity = await _context.Set<T>().FindAsync(id)
            ?? throw new ItemNotFoundException($"Item with {id} is not found");
        _context.Set<T>().Remove(entity);
        return await _context.SaveChangesAsync();
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