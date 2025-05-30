using Microsoft.AspNetCore.Identity;
using PrimeCare.Core.Entities.AiEntites;

namespace PrimeCare.Core.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = null!;
        public Address? Address { get; set; }


    }
}
