using Microsoft.EntityFrameworkCore;
using PrimeCare.Core.Entities;
using PrimeCare.Core.Specifications;

namespace PrimeCare.Infrastructure.Data;

/// <summary>
/// Evaluates specifications and applies them to queries.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
{
    /// <summary>
    /// Applies the given specification to the input query.
    /// </summary>
    /// <param name="inputQuery">The input query to which the specification will be applied.</param>
    /// <param name="specification">The specification containing criteria and includes to apply to the query.</param>
    /// <returns>The query with the specification applied.</returns>
    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,
        ISpecification<TEntity> specification)
    {
        var query = inputQuery;

        // Apply the criteria if it exists
        if (specification.Criteria != null)
            query = query.Where(specification.Criteria);

        if (specification.OrderBy != null)
            query = query.OrderBy(specification.OrderBy);
        else if (specification.OrderByDescending != null)
            query = query.OrderByDescending(specification.OrderByDescending);

        // Apply the includes to the query
        query = specification.Includes.Aggregate(query,
            (current, include) => current.Include(include));

        return query;
    }
}
