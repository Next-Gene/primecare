using PrimeCare.Core.Entities;
using PrimeCare.Core.Specifications;

namespace PrimeCare.Core.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<T?> GetEntityWithSpecification(ISpecification<T> specification);
    Task<IReadOnlyList<T>> GetAllWithSpecificationAsync(ISpecification<T> specification);
    Task<int> AddAsync(T entity);
    Task<int> UpdateAsync(T entity);
    Task<int> DeleteAsync(int id);
}
