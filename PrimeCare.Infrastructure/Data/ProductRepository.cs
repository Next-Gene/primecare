using Microsoft.EntityFrameworkCore;
using PrimeCare.Core.Entities;
using PrimeCare.Core.Interfaces;

namespace PrimeCare.Infrastructure.Data;

public class ProductRepository : IProductRepository
{
    private readonly PrimeCareContext _context;

    public ProductRepository(PrimeCareContext context)
    {
        _context = context;
    }

    public async Task<Product> GetProductByIdAsync(int id)
        => await _context.Products.FindAsync(id);

    public async Task<IReadOnlyList<Product>> GetProductsAsync()
        => await _context.Products.ToListAsync();
}
