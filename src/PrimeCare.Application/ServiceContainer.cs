using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PrimeCare.Api.Helpers;
using PrimeCare.Application.Services.Implementations;
using PrimeCare.Application.Services.Interfaces;

namespace PrimeCare.Application;

/// <summary>
/// Provides extension methods for registering application services in the dependency injection container.
/// </summary>
public static class ServiceContainer
{
    /// <summary>
    /// Adds application-level services to the specified <see cref="IServiceCollection"/>.
    /// Registers AutoMapper profiles and service implementations for products, brands, categories, photos, carts, and wishlists.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <returns>The service collection with the added services.</returns>
    public static IServiceCollection AddApplicationService
       (this IServiceCollection services)
    {
        services.AddSingleton<IResponseCacheService, ResponseCacheService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddAutoMapper(typeof(MappingProfiles));
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IProductBrandService, ProductBrandService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IPhotoService, PhotoService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IWishlistService, WishlistService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddHttpClient<IMedicalAIService, MedicalAIService>();
        services.AddScoped<IMedicalAIService, MedicalAIService>();

     
        return services;
    }
}
