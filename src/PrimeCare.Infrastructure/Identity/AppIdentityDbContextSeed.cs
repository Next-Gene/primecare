using Microsoft.AspNetCore.Identity;
using PrimeCare.Core.Entities.Identity;

namespace PrimeCare.Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Seed roles first
            await SeedRolesAsync(roleManager);

            // Seed users with roles
            await SeedUserAsync(userManager);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            // Updated to include "User" as the default role
            var roles = new[] { "Admin", "Seller", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private static async Task SeedUserAsync(UserManager<ApplicationUser> userManager)
        {
            // Create Admin user
            if (await userManager.FindByEmailAsync("seifmoataz27249@gmail.com") == null)
            {
                var adminUser = new ApplicationUser
                {
                    FullName = "Admin User",
                    UserName = "admin",
                    Email = "seifmoataz27249@gmail.com",
                    PhoneNumber = "01022832634",
                    Address = new Address
                    {
                        FirstName = "Admin",
                        LastName = "User",
                        Street = "Admin Street",
                        City = "Cairo",
                        State = "Cairo",
                        ZipCode = "12345"
                    }
                };

                var result = await userManager.CreateAsync(adminUser, "Pa$$w0rd");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Create Seller user
            if (await userManager.FindByEmailAsync("seifeng2912@gmail.com") == null)
            {
                var sellerUser = new ApplicationUser
                {
                    FullName = "Seller User",
                    UserName = "seller",
                    Email = "seifeng2912@gmail.com",
                    PhoneNumber = "01122334455",
                    Address = new Address
                    {
                        FirstName = "Seller",
                        LastName = "User",
                        Street = "Seller Street",
                        City = "Cairo",
                        State = "Cairo",
                        ZipCode = "54321"
                    }
                };

                var result = await userManager.CreateAsync(sellerUser, "Pa$$w0rd");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(sellerUser, "Seller");
                }
            }

            // Create a regular User for testing
            if (await userManager.FindByEmailAsync("user@example.com") == null)
            {
                var regularUser = new ApplicationUser
                {
                    FullName = "Regular User",
                    UserName = "user",
                    Email = "user@example.com",
                    PhoneNumber = "01000000000",
                    Address = new Address
                    {
                        FirstName = "Regular",
                        LastName = "User",
                        Street = "User Street",
                        City = "Cairo",
                        State = "Cairo",
                        ZipCode = "11111"
                    }
                };

                var result = await userManager.CreateAsync(regularUser, "Pa$$w0rd");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(regularUser, "User");
                }
            }

            // Update existing user to have a role if needed
            var existingUser = await userManager.FindByEmailAsync("seifemam");
            if (existingUser != null)
            {
                var userRoles = await userManager.GetRolesAsync(existingUser);
                if (!userRoles.Any())
                {
                    await userManager.AddToRoleAsync(existingUser, "Admin");
                }
            }
        }
    }
}