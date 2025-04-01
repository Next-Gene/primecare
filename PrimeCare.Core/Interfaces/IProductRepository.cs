using PrimeCare.Core.Entities;

namespace PrimeCare.Core.Interfaces;

public interface IProductRepository
{
    Task<Product> GetProductByIdAsync(int id);
    Task<IReadOnlyList<Product>> GetProductsAsync();
}
