using AutoMapper;
using PrimeCare.Application.Dtos.Categories;
using PrimeCare.Application.Dtos.Products;
using PrimeCare.Application.Dtos.ProductBrand;
using PrimeCare.Core.Entities;
using PrimeCare.Application.Dtos.Photos;

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
            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.ProductPhotos.FirstOrDefault(X => X.IsMain).Url));

        CreateMap<ProductBrand, ProductBrandDto>();
        CreateMap<ProductBrandDto, ProductBrand>();
        CreateMap<CreateProductBrandDto, ProductBrand>();

        CreateMap<Category, CategoryDto>()
         .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.CategoryPhoto.Url));

        CreateMap<UpdateCategoryDto, Category>();

        CreateMap<CreateCategoryDto, Category>();

        CreateMap< CategoryPhoto, CategoryPhotoDto>();
        CreateMap< ProductPhotos, ProductPhotosDto>();


    }
}