using Microsoft.OpenApi.Models;

namespace PrimeCare.Api.Extensions;

/// <summary>
/// Provides extension methods for configuring Swagger/OpenAPI documentation services and middleware.
/// </summary>
public static class SwaggerServiceExtension
{
    /// <summary>
    /// Adds Swagger/OpenAPI documentation generation services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to add the Swagger services to.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> with Swagger services registered.</returns>
    public static IServiceCollection AddSwaggerDecumentation
        (this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "PrimeCare API", Version = "v1" });
        });

        return services;
    }

    /// <summary>
    /// Configures the application to use Swagger and Swagger UI middleware for API documentation.
    /// </summary>
    /// <param name="app">The application builder to configure.</param>
    /// <returns>The updated <see cref="IApplicationBuilder"/> with Swagger middleware configured.</returns>
    public static IApplicationBuilder UseSwaggerDecumentation
    (this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "PrimeCare API v1");
        });

        return app;
    }
}
