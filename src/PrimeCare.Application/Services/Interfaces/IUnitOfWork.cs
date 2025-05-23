using PrimeCare.Core.Entities;
using PrimeCare.Core.Interfaces;

namespace PrimeCare.Infrastructure.Repositories
{
    public interface IUnitOfWork
    {
        Task<int> Complete();
        void Dispose();
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
    }
}
