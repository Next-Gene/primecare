using Microsoft.EntityFrameworkCore;
using PrimeCare.Core.Entities;
using PrimeCare.Core.Interfaces;

namespace PrimeCare.Infrastructure.Data;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly PrimeCareContext _context;

    public GenericRepository(PrimeCareContext context)
    {
        _context = context;
    }

    public async Task<T?> GetByIdAsync(int id)
        => await _context.Set<T>().FindAsync(id);

    public async Task<IReadOnlyList<T>> ListAllAsync()
        => await _context.Set<T>().ToListAsync();
}
