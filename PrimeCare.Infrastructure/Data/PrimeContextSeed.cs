using Microsoft.Extensions.Logging;
using PrimeCare.Core.Entities;
using System.Text.Json;

namespace PrimeCare.Infrastructure.Data;

public class PrimeContextSeed
{
    public static async Task SeedAsync(PrimeCareContext context, ILoggerFactory loggerFactory)
    {
        try
        {
            if (!context.ProductBrands.Any())
            {
                var brandsData = File.ReadAllText(@"..\PrimeCare.Infrastructure\Data\SeedData\brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                foreach (var brand in brands!)
                    context.ProductBrands.Add(brand);
                await context.SaveChangesAsync();
            }

            if (!context.ProductTypes.Any())
            {
                var typesData = File.ReadAllText(@"..\PrimeCare.Infrastructure\Data\SeedData\types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                foreach (var type in types!)
                    context.ProductTypes.Add(type);
                await context.SaveChangesAsync();
            }

            if (!context.Products.Any())
            {
                var productsData = File.ReadAllText(@"..\PrimeCare.Infrastructure\Data\SeedData\products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                foreach (var product in products!)
                    context.Products.Add(product);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<PrimeContextSeed>();
            logger.LogError(ex.Message);
        }
    }
}
