using AutoMapper;
using Microsoft.Extensions.Configuration;
using PrimeCare.Application.Dtos.Product;
using PrimeCare.Core.Entities;

namespace PrimeCare.Api.Helpers;

/// <summary>
/// Resolves the full URL for a product's picture.
/// </summary>
public class ProductUrlResolver : IValueResolver<Product, ProductDto, string>
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductUrlResolver"/> class.
    /// </summary>
    /// <param name="configuration">The configuration to use for resolving the URL.</param>
    public ProductUrlResolver(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Resolves the full URL for a product's picture.
    /// </summary>
    /// <param name="source">The source product entity.</param>
    /// <param name="destination">The destination product DTO.</param>
    /// <param name="destMember">The destination member being resolved.</param>
    /// <param name="context">The resolution context.</param>
    /// <returns>The full URL for the product's picture.</returns>
    public string Resolve(Product source, ProductDto destination,
        string destMember, ResolutionContext context)
    {
        if (!string.IsNullOrEmpty(source.PictureUrl))
            return _configuration["ApiUrl"] + source.PictureUrl;

        return null!;
    }
}