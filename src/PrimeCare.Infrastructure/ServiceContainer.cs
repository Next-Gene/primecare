using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrimeCare.Core.Interfaces;
using PrimeCare.Infrastructure.Data;
using PrimeCare.Infrastructure.Repositories;
using StackExchange.Redis;

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

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddSingleton<ConnectionMultiplexer>(c =>
        {
            var config = ConfigurationOptions.Parse(configuration.GetConnectionString("Redis"), true);

            return ConnectionMultiplexer.Connect(config);
        });
        return services;
    }
}