using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrimeCare.Api.Extensions;
using PrimeCare.Application;
using PrimeCare.Application.Middleware;
using PrimeCare.Core.Entities.Identity;
using PrimeCare.Infrastructure;
using PrimeCare.Infrastructure.Data;
using PrimeCare.Infrastructure.Identity;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container
        builder.Services.AddControllers()
        .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
        builder.Services.AddSwaggerDecumentation();

        builder.Services.AddApplicationService(); // for application layer
        builder.Services.AddInfrastructureService(builder.Configuration);
        builder.Services.AddIdentityServices(builder.Configuration); // ✅ Identity Services
        builder.Services.AddApplicationServices(builder.Configuration); // for API layer

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerDecumentation();
        }

        app.UseMiddleware<ExceptionMiddleware>();
        app.UseStatusCodePagesWithReExecute("/error/{0}");

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseCors("AllowAll");

        app.UseAuthentication(); // ✅ مهم جداً
        app.UseAuthorization();

        app.MapControllers();

        // Apply migrations
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();

        try
        {
            var context = services.GetRequiredService<PrimeCareContext>();

            await context.Database.MigrateAsync();
            await PrimeContextSeed.SeedAsync(context, loggerFactory);


            var identityContext = services.GetRequiredService<AppIdentityDbContext>();
            await identityContext.Database.MigrateAsync();

            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            await AppIdentityDbContextSeed.SeedUserAsync(userManager); // Uncomment for seeding
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogError(ex, "An error occurred during migration");
        }

        app.Run();
    }
}
