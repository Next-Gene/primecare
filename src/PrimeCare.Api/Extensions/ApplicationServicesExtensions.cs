using Microsoft.AspNetCore.Mvc;
using PrimeCare.Application.Helpers;
using PrimeCare.Shared.Errors;

namespace PrimeCare.Api.Extensions;

/// <summary>
/// Provides extension methods for registering application-level services and configurations.
/// </summary>
public static class ApplicationServicesExtensions
{
    /// <summary>
    /// Adds application services and configures model validation and Cloudinary settings.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <param name="configuration">The application configuration for accessing settings.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> with the registered services.</returns>
    public static IServiceCollection AddApplicationServices
        (this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ApiBehaviorOptions>(option =>
        {
            option.InvalidModelStateResponseFactory = actionContext =>
            {
                var errors = actionContext.ModelState
                    .Where(e => e.Value!.Errors.Count > 0)
                    .SelectMany(x => x.Value!.Errors)
                    .Select(x => x.ErrorMessage);

                var errorResponse = new ApiValidationErrorResponse
                {
                    Errors = errors
                };

                return new BadRequestObjectResult(errorResponse);
            };
        });

        services.Configure<CloudinarySettings>(configuration
            .GetSection("CloudinarySettings"));

        return services;
    }
}
