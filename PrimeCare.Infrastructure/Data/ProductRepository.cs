using Microsoft.EntityFrameworkCore;
using PrimeCare.Core.Entities;
using PrimeCare.Core.Interfaces;

namespace PrimeCare.Infrastructure.Data;

/// <summary>
/// Repository for managing product data.
/// </summary>
public class ProductRepository : IProductRepository
{
    private readonly PrimeCareContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public ProductRepository(PrimeCareContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets a product by its identifier.
    /// </summary>
    /// <param name="id">The product identifier.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the product.</returns>
    public async Task<Product> GetProductByIdAsync(int id)
        => await _context.Products.FindAsync(id);

    /// <summary>
    /// Gets all products.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of products.</returns>
    public async Task<IReadOnlyList<Product>> GetProductsAsync()
        => await _context.Products.ToListAsync();
}
