using Microsoft.EntityFrameworkCore;
using PrimeCare.Core.Entities;
using PrimeCare.Core.Specifications;

namespace PrimeCare.Infrastructure.Data;

/// <summary>
/// Provides functionality to evaluate and apply specifications to queries for entities.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
{
    /// <summary>
    /// Applies the given specification to the input query, including filtering, sorting, paging, and eager loading.
    /// </summary>
    /// <param name="inputQuery">The input query to which the specification will be applied.</param>
    /// <param name="specification">The specification containing criteria, includes, sorting, and paging information.</param>
    /// <returns>
    /// An <see cref="IQueryable{TEntity}"/> with the specification applied, ready for execution against the data source.
    /// </returns>
    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,
        ISpecification<TEntity> specification)
    {
        var query = inputQuery;

        // Apply the criteria if it exists
        if (specification.Criteria != null)
            query = query.Where(specification.Criteria);

        // Apply ordering if specified
        if (specification.OrderBy != null)
            query = query.OrderBy(specification.OrderBy);
        if (specification.OrderByDescending != null)
            query = query.OrderByDescending(specification.OrderByDescending);

        // Apply paging if enabled
        if (specification.IsPagingEnabled)
            query = query.Skip(specification.Skip).Take(specification.Take);

        // Apply eager loading for related entities
        query = specification.Includes.Aggregate(query,
            (current, include) => current.Include(include));

        return query;
    }
}
