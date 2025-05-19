using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrimeCare.Core.Entities.Identity;

namespace PrimeCare.Api.Extensions
{
    public static class UserMangerExtensions
    {

        public static async Task<ApplicationUser> FinddUserByClaimsPrincipallWithAddressAsync(this UserManager<ApplicationUser> input, ClaimsPrincipal user)
        {

            var email = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            return await input.Users
                .Include(x => x.Address)
                .SingleOrDefaultAsync(x => x.Email == email);
        }


        public static async Task<ApplicationUser> FindEmailFromPrincipal(this UserManager<ApplicationUser> input, ClaimsPrincipal user)
        {

            var email = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            return await input.Users
                .SingleOrDefaultAsync(x => x.Email == email);


        }



    }
}
