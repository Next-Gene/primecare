using Microsoft.EntityFrameworkCore;
using PrimeCare.Api.Extensions;
using PrimeCare.Application;
using PrimeCare.Application.Middleware;
using PrimeCare.Infrastructure;
using PrimeCare.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerDecumentation();

builder.Services.AddApplicationService(); // for application layer
builder.Services.AddInfrastructureService(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration); // for api layer

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDecumentation();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseStatusCodePagesWithReExecute("/error/{0}");

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var loggerFactory = services.GetRequiredService<ILoggerFactory>();
try
{
    var context = services.GetRequiredService<PrimeCareContext>();
    await context.Database.MigrateAsync();
    //await PrimeContextSeed.SeedAsync(context, loggerFactory);
}
catch (Exception ex)
{
    var logger = loggerFactory.CreateLogger<Program>();
    logger.LogError(ex, "An error occurred during migration");
}

app.Run();
