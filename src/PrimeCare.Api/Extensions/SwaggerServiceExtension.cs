using Microsoft.OpenApi.Models;

namespace PrimeCare.Api.Extensions;

public static class SwaggerServiceExtension
{
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
