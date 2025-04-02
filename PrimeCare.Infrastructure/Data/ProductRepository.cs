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
    /// Gets all product brands.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of product brands.</returns>
    public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        => await _context.ProductBrands.ToListAsync();

    /// <summary>
    /// Gets a product by its identifier.
    /// </summary>
    /// <param name="id">The product identifier.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the product.</returns>
    public async Task<Product?> GetProductByIdAsync(int? id)
        => await _context.Products
        .Include(p => p.ProductBrand)
        .Include(p => p.ProductType)
        .FirstOrDefaultAsync(p => p.Id == id);

    /// <summary>
    /// Gets all products.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of products.</returns>
    public async Task<IReadOnlyList<Product>> GetProductsAsync()
        => await _context.Products
        .Include(p => p.ProductBrand)
        .Include(p => p.ProductType)
        .ToListAsync();

    /// <summary>
    /// Gets all product types.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of product types.</returns>
    public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
        => await _context.ProductTypes.ToListAsync();
}
