using Microsoft.EntityFrameworkCore;
using PrimeCare.Core.Entities;

namespace PrimeCare.Infrastructure.Data;

public class PrimeCareContext : DbContext
{
    public PrimeCareContext(DbContextOptions<PrimeCareContext> options)
        : base(options)
    {

    }
    public DbSet<Product> Products { get; set; }
}
