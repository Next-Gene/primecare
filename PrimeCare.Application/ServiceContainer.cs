﻿using Microsoft.Extensions.DependencyInjection;
using PrimeCare.Api.Helpers;
using PrimeCare.Application.Services.Implementations;
using PrimeCare.Application.Services.Interfaces;

namespace PrimeCare.Application;

public static class ServiceContainer
{
    public static IServiceCollection AddApplicationService
       (this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfiles));
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IProductBrandService, ProductBrandService>();
        services.AddScoped<ICategoryService, CategoryService>();
        return services;
    }
}
