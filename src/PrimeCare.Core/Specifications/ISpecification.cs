using System.Linq.Expressions;

namespace PrimeCare.Core.Specifications;

/// <summary>
/// Defines a contract for building query specifications, including filtering, eager loading, sorting, and paging.
/// </summary>
/// <typeparam name="T">The type of the entity being queried.</typeparam>
public interface ISpecification<T>
{
    /// <summary>
    /// Gets the filtering criteria expression used to filter entities.
    /// </summary>
    Expression<Func<T, bool>> Criteria { get; }

    /// <summary>
    /// Gets the list of expressions used to include related entities in the query.
    /// These expressions define navigation properties to be eagerly loaded.
    /// </summary>
    List<Expression<Func<T, object>>> Includes { get; }

    /// <summary>
    /// Gets the expression that defines the ascending order for the query results.
    /// </summary>
    Expression<Func<T, object>> OrderBy { get; }

    /// <summary>
    /// Gets the expression that defines the descending order for the query results.
    /// </summary>
    Expression<Func<T, object>> OrderByDescending { get; }

    /// <summary>
    /// Gets the number of records to take (used for pagination).
    /// </summary>
    int Take { get; }

    /// <summary>
    /// Gets the number of records to skip (used for pagination).
    /// </summary>
    int Skip { get; }

    /// <summary>
    /// Gets a value indicating whether pagination is enabled for the query.
    /// </summary>
    bool IsPagingEnabled { get; }
}
