using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrimeCare.Application.Services.Implementations;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities;
using PrimeCare.Core.Interfaces;
using PrimeCare.Infrastructure.Data;
using PrimeCare.Infrastructure.Identity;
using PrimeCare.Infrastructure.Repositories;
using StackExchange.Redis;

namespace PrimeCare.Infrastructure;

public static class ServiceContainer
{
    public static IServiceCollection AddInfrastructureService
       (this IServiceCollection services, IConfiguration configuration)
    {
        // Database Contexts
        services.AddDbContext<PrimeCareContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Default"));
        });

        services.AddDbContext<AppIdentityDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Identity"));
        });

        // Repositories
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        // Redis
        services.AddSingleton<IConnectionMultiplexer>(c =>
        {
            var config = ConfigurationOptions.Parse(configuration.GetConnectionString("Redis"), true);
            return ConnectionMultiplexer.Connect(config);
        });

        // Email Settings and Email Service
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
