using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrimeCare.Core.Entities;
using PrimeCare.Core.Interfaces;
using PrimeCare.Infrastructure.Data;
using PrimeCare.Infrastructure.Repositories;

namespace PrimeCare.Infrastructure;

/// <summary>
/// Provides extension methods for adding infrastructure services to the service collection.
/// </summary>
public static class ServiceContainer
{
    /// <summary>
    /// Adds the infrastructure services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <param name="configuration">The configuration to use for setting up the services.</param>
    /// <returns>The service collection with the added services.</returns>
    public static IServiceCollection AddInfrastructureService
       (this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PrimeCareContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Default"));
        });

        services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();
        services.AddScoped<IGenericRepository<ProductBrand>, GenericRepository<ProductBrand>>();
        services.AddScoped<IGenericRepository<ProductType>, GenericRepository<ProductType>>();

        return services;
    }
}