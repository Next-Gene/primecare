using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrimeCare.Core.Entities;
using PrimeCare.Core.Interfaces;
using PrimeCare.Infrastructure.Data;
using PrimeCare.Infrastructure.Repositories;

namespace PrimeCare.Infrastructure;

public static class ServiceContainer
{
    public static IServiceCollection AddInfrastructureService
       (this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PrimeCareContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Default"));
        });

        services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();

        return services;
    }
}
