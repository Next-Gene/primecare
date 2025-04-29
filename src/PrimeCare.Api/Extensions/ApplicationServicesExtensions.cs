using Microsoft.AspNetCore.Mvc;
using PrimeCare.Application.Helpers;
using PrimeCare.Shared.Errors;

namespace PrimeCare.Api.Extensions;

public static class ApplicationServicesExtensions
{
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
