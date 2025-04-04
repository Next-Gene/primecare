using AutoMapper;
using Microsoft.Extensions.Configuration;
using PrimeCare.Application.Dtos.Product;
using PrimeCare.Core.Entities;

namespace PrimeCare.Api.Helpers;

public class ProductUrlResolver : IValueResolver<Product, ProductDto, string>
{
    private readonly IConfiguration _configuration;

    public ProductUrlResolver(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Resolve(Product source, ProductDto destination,
        string destMember, ResolutionContext context)
    {
        if (!string.IsNullOrEmpty(source.PictureUrl))
            return _configuration["ApiUrl"] + source.PictureUrl;

        return null!;
    }
}
