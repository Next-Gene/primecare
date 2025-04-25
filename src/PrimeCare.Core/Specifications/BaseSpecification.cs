using System.Linq.Expressions;

namespace PrimeCare.Core.Specifications;

/// <summary>
/// Base class for specifications, used to encapsulate query criteria and includes.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public class BaseSpecification<T> : ISpecification<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseSpecification{T}"/> class.
    /// </summary>
    /// <param name="criteria">The criteria expression to filter entities.</param>
    public BaseSpecification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    /// <summary>
    /// Gets the criteria expression to filter entities.
    /// </summary>
    public Expression<Func<T, bool>> Criteria { get; }

    /// <summary>
    /// Gets the list of expressions used to include related entities in the query.
    /// </summary>
    public List<Expression<Func<T, object>>> Includes { get; } = new();

    /// <summary>
    /// Gets the expression that defines the ascending order for the query result.
    /// </summary>
    public Expression<Func<T, object>> OrderBy { get; private set; }

    /// <summary>
    /// Gets the expression that defines the descending order for the query result.
    /// </summary>
    public Expression<Func<T, object>> OrderByDescending { get; private set; }

    #region Methods
    /// <summary>
    /// Adds an include expression to the list of includes.
    /// </summary>
    /// <param name="includeExpression">The include expression.</param>
    protected void AddInclude(Expression<Func<T, object>> includeExpression)
       => Includes.Add(includeExpression);

    /// <summary>
    /// Applies an ascending order expression to the specification query.
    /// </summary>
    /// <param name="orderByExpression">The expression used to order the results in ascending order.</param>
    protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        => OrderBy = orderByExpression;

    /// <summary>
    /// Applies a descending order expression to the specification query.
    /// </summary>
    /// <param name="orderByDescExpression">The expression used to order the results in descending order.</param>
    protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
        => OrderByDescending = orderByDescExpression;

    #endregion
}
