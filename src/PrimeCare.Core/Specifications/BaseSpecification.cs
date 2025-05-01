using System.Linq.Expressions;

namespace PrimeCare.Core.Specifications;

/// <summary>
/// A base class for building specifications, which encapsulate query criteria, includes, and sorting logic.
/// This class is designed to simplify the creation of reusable and composable query definitions.
/// </summary>
/// <typeparam name="T">The type of the entity being queried.</typeparam>
public class BaseSpecification<T> : ISpecification<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseSpecification{T}"/> class with a filtering criteria.
    /// </summary>
    /// <param name="criteria">The expression used to filter entities.</param>
    public BaseSpecification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    /// <summary>
    /// Gets the filtering criteria expression used to filter entities.
    /// </summary>
    public Expression<Func<T, bool>> Criteria { get; }

    /// <summary>
    /// Gets the list of expressions used to include related entities in the query.
    /// These expressions define navigation properties to be eagerly loaded.
    /// </summary>
    public List<Expression<Func<T, object>>> Includes { get; } = new();

    /// <summary>
    /// Gets the expression that defines the ascending order for the query results.
    /// </summary>
    public Expression<Func<T, object>> OrderBy { get; private set; }

    /// <summary>
    /// Gets the expression that defines the descending order for the query results.
    /// </summary>
    public Expression<Func<T, object>> OrderByDescending { get; private set; }

    /// <summary>
    /// Gets the number of records to take (used for pagination).
    /// </summary>
    public int Take { get; private set; }

    /// <summary>
    /// Gets the number of records to skip (used for pagination).
    /// </summary>
    public int Skip { get; private set; }

    /// <summary>
    /// Gets a value indicating whether pagination is enabled for the query.
    /// </summary>
    public bool IsPagingEnabled { get; private set; }

    #region Methods

    /// <summary>
    /// Adds an include expression to the list of includes, specifying a related entity to be eagerly loaded.
    /// </summary>
    /// <param name="includeExpression">The expression representing the related entity to include.</param>
    protected void AddInclude(Expression<Func<T, object>> includeExpression)
        => Includes.Add(includeExpression);

    /// <summary>
    /// Applies an ascending order expression to the query.
    /// </summary>
    /// <param name="orderByExpression">The expression used to order the results in ascending order.</param>
    protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        => OrderBy = orderByExpression;

    /// <summary>
    /// Applies a descending order expression to the query.
    /// </summary>
    /// <param name="orderByDescExpression">The expression used to order the results in descending order.</param>
    protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
        => OrderByDescending = orderByDescExpression;

    /// <summary>
    /// Enables pagination for the query by specifying the number of records to skip and take.
    /// </summary>
    /// <param name="skip">The number of records to skip.</param>
    /// <param name="take">The number of records to take.</param>
    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }

    #endregion
}