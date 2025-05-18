using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PrimeCare.Core.Entities.Identity;

namespace PrimeCare.Infrastructure.Identity
{
    public class AppIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Address)
                .WithOne(a => a.AppUser)
                .HasForeignKey<Address>(a => a.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }

}

