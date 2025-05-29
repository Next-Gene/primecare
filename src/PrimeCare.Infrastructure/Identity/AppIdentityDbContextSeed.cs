using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using PrimeCare.Core.Entities;
using PrimeCare.Core.Entities.Identity;
using PrimeCare.Infrastructure.Data; // تأكد من مسار الـ DbContext

namespace PrimeCare.Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            PrimeCareContext context)
        {
            // 1. Seed roles
            await SeedRolesAsync(roleManager);

            // 2. Seed users (Admin, Seller, User)
            await SeedUsersAsync(userManager);

            // 3. Seed products linked to seller
            await SeedProductsAsync(userManager, context);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            var roles = new[] { "Admin", "Seller", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
        {
            // Admin user
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

            // Seller user
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

            // Regular user
            if (await userManager.FindByEmailAsync("regularuser@example.com") == null)
            {
                var regularUser = new ApplicationUser
                {
                    FullName = "Regular User",
                    UserName = "regularuser",
                    Email = "regularuser@example.com",
                    PhoneNumber = "01234567890",
                    Address = new Address
                    {
                        FirstName = "Regular",
                        LastName = "User",
                        Street = "User Street",
                        City = "Alexandria",
                        State = "Alexandria",
                        ZipCode = "67890"
                    }
                };

                var result = await userManager.CreateAsync(regularUser, "Pa$$w0rd");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(regularUser, "User");
                }
            }
        }

        private static async Task SeedProductsAsync(UserManager<ApplicationUser> userManager, PrimeCareContext context)
        {
            if (!context.Products.Any())
            {
                var productsData = File.ReadAllText(@"..\PrimeCare.Infrastructure\Data\SeedData\products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                var sellerUser = await userManager.FindByEmailAsync("seifeng2912@gmail.com");
                if (sellerUser == null)
                {
                    throw new Exception("Seller user not found!");
                }

                foreach (var product in products!)
                {
                    product.CreatedBy = sellerUser.Id;
                    product.CreatedByName = sellerUser.FullName;
                }
                context.Products.AddRange(products);
                await context.SaveChangesAsync();
            }
        }
    }
}
