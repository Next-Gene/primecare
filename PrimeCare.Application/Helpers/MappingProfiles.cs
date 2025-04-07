using AutoMapper;
using PrimeCare.Application.Dtos.Product;
using PrimeCare.Application.Dtos.ProductBrand;
using PrimeCare.Application.Dtos.ProductType;
using PrimeCare.Core.Entities;

namespace PrimeCare.Api.Helpers;

/// <summary>
/// Defines the mapping profiles for AutoMapper.
/// </summary>
public class MappingProfiles : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MappingProfiles"/> class.
    /// </summary>
    public MappingProfiles()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.ProductBrand, opt => opt.MapFrom(src => src.ProductBrand.Name))
            .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType.Name))
            .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<ProductUrlResolver>());

        CreateMap<ProductBrand, ProductBrandDto>();
        CreateMap<ProductBrandDto, ProductBrand>();
        CreateMap<CreateProductBrandDto, ProductBrand>();

        CreateMap<ProductType, ProductTypeDto>();
        CreateMap<ProductTypeDto, ProductType>();
        CreateMap<CreateProductTypeDto, ProductType>();
    }
}