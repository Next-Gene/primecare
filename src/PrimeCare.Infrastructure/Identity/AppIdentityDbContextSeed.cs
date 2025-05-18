using Microsoft.AspNetCore.Identity;
using PrimeCare.Core.Entities.Identity;

namespace PrimeCare.Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {

        public static async Task SeedUserAsync(UserManager<ApplicationUser> UserManager)
        {

            if (!UserManager.Users.Any())
            {
                var user = new ApplicationUser
                {
                    FullName = "SeifEmam",
                    UserName = "seifemam",
                    Email = "seifmoataz27249@gmail.com",
                    PhoneNumber = "01022832634",
                    Address = new Address
                    {
                        FirstName = "Seif",
                        LastName = "Emam",
                        Street = "Street 1",
                        City = "Cairo",
                        State = "Cairo",
                        ZipCode = "12345"
                    }
                };

                await UserManager.CreateAsync(user, "Pa$$w0rd");

            }

        }
    }
}
