using AutoMapper;
using PrimeCare.Application.Dtos.Product;
using PrimeCare.Application.Dtos.ProductBrand;
using PrimeCare.Core.Entities;

namespace PrimeCare.Api.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.ProductBrand, opt => opt.MapFrom(src => src.ProductBrand.Name))
            .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType.Name))
            .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<ProductUrlResolver>());

        //CreateMap<ProductDto, Product>()
        //   .ForMember(dest => dest.ProductBrand.Name, opt => opt.MapFrom(src => src.ProductBrand))
        //   .ForMember(dest => dest.ProductType.Name, opt => opt.MapFrom(src => src.ProductType));

        CreateMap<ProductBrand, ProductBrandDto>();
        CreateMap<ProductBrand, CreateProductBrandDto>();

    }
}
