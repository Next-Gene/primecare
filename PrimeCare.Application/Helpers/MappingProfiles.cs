using AutoMapper;
using PrimeCare.Application.Dtos.Category;
using PrimeCare.Application.Dtos.Product;
using PrimeCare.Application.Dtos.ProductBrand;
using PrimeCare.Application.Helpers;
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
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<ProductUrlResolver>());

        CreateMap<ProductBrand, ProductBrandDto>();
        CreateMap<ProductBrandDto, ProductBrand>();
        CreateMap<CreateProductBrandDto, ProductBrand>();

        CreateMap<Category, CategoryDto>()
             .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom<CategoryUrlResolver>());
        CreateMap<UpdateCategoryDto, Category>();
        CreateMap<CreateCategoryDto, Category>();
    }
}