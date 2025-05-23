using System.Text.Json;
using Microsoft.Extensions.Logging;
using PrimeCare.Core.Entities.Order;

namespace PrimeCare.Infrastructure.Data;

/// <summary>
/// Provides methods to seed the PrimeCare database with initial data for brands, categories, and products.
/// </summary>
public class PrimeContextSeed
{
    /// <summary>
    /// Seeds the database with initial data from JSON files if the corresponding tables are empty.
    /// </summary>
    /// <param name="context">The <see cref="PrimeCareContext"/> used to access the database.</param>
    /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> used for logging errors.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task SeedAsync(PrimeCareContext context, ILoggerFactory loggerFactory)
    {
        try
        {

            if (!context.DeliveryMethods.Any())
            {
                var dmData = File.ReadAllText(@"..\PrimeCare.Infrastructure\Data\SeedData\delivery.json");

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(dmData, options);

                foreach (var item in methods!)
                {
                    context.DeliveryMethods.Add(item);
                }
                await context.SaveChangesAsync();
            }

            //// Seed product brands if none exist
            //if (!context.ProductBrands.Any())
            //{
            //    var brandsData = File.ReadAllText(@"..\PrimeCare.Infrastructure\Data\SeedData\brands.json");
            //    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
            //    foreach (var brand in brands!)
            //        context.ProductBrands.Add(brand);
            //    await context.SaveChangesAsync();
            //}

            //// Seed categories if none exist
            //if (!context.Categories.Any())
            //{
            //    var categoriesData = File.ReadAllText(@"..\PrimeCare.Infrastructure\Data\SeedData\categories.json");
            //    var categories = JsonSerializer.Deserialize<List<Category>>(categoriesData);
            //    foreach (var category in categories!)
            //        context.Categories.Add(category);
            //    await context.SaveChangesAsync();
            //}

            //// Seed products if none exist
            //if (!context.Products.Any())
            //{
            //    var productsData = File.ReadAllText(@"..\PrimeCare.Infrastructure\Data\SeedData\products.json");
            //    var products = JsonSerializer.Deserialize<List<Product>>(productsData);
            //    foreach (var product in products!)
            //        context.Products.Add(product);
            //    await context.SaveChangesAsync();
            //}


        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<PrimeContextSeed>();
            logger.LogError(ex.Message);
        }
    }
}
