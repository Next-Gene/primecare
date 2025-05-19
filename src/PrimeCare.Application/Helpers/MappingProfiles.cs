using AutoMapper;
using PrimeCare.Core.Entities;
using PrimeCare.Core.Entities.Identity;
using PrimeCare.Shared.Dtos.Categories;
using PrimeCare.Shared.Dtos.Photos;
using PrimeCare.Shared.Dtos.ProductBrand;
using PrimeCare.Shared.Dtos.Products;
using PrimeCare.Shared.Dtos.User;

namespace PrimeCare.Api.Helpers;

/// <summary>
/// Defines the mapping profiles for AutoMapper.
/// </summary>
public class MappingProfiles : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MappingProfiles"/> class.
    /// Configures mappings between domain entities and DTOs for products, brands, categories, and photos.
    /// </summary>
    public MappingProfiles()
    {
        /// <summary>
        /// Maps <see cref="Product"/> to <see cref="ProductDto"/>.
        /// Maps brand and category names, and selects the main product photo URL.
        /// </summary>
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.ProductBrand, opt => opt.MapFrom(src => src.ProductBrand.Name))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.ProductPhotos.FirstOrDefault(x => x.IsMain)!.Url));

        /// <summary>
        /// Maps <see cref="CreateProductDto"/> to <see cref="Product"/>.
        /// </summary>
        CreateMap<CreateProductDto, Product>();

        /// <summary>
        /// Maps <see cref="UpdateProductDto"/> to <see cref="Product"/>.
        /// </summary>
        CreateMap<UpdateProductDto, Product>();

        /// <summary>
        /// Maps <see cref="ProductPhoto"/> to <see cref="ProductPhotoDto"/>.
        /// </summary>
        CreateMap<ProductPhoto, ProductPhotoDto>();

        /// <summary>
        /// Maps <see cref="ProductBrand"/> to <see cref="ProductBrandDto"/> and vice versa.
        /// </summary>
        CreateMap<ProductBrand, ProductBrandDto>();
        CreateMap<ProductBrandDto, ProductBrand>();

        /// <summary>
        /// Maps <see cref="CreateProductBrandDto"/> to <see cref="ProductBrand"/>.
        /// </summary>
        CreateMap<CreateProductBrandDto, ProductBrand>();

        /// <summary>
        /// Maps <see cref="Category"/> to <see cref="CategoryDto"/>.
        /// Maps the main category photo URL.
        /// </summary>
        CreateMap<Category, CategoryDto>()
            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.CategoryPhotos.FirstOrDefault(x => x.IsMain)!.Url));

        /// <summary>
        /// Maps <see cref="UpdateCategoryDto"/> to <see cref="Category"/>.
        /// </summary>
        CreateMap<UpdateCategoryDto, Category>();

        /// <summary>
        /// Maps <see cref="CreateCategoryDto"/> to <see cref="Category"/>.
        /// </summary>
        CreateMap<CreateCategoryDto, Category>();

        /// <summary>
        /// Maps <see cref="CategoryPhoto"/> to <see cref="CategoryPhotoDto"/>.
        /// </summary>
        CreateMap<CategoryPhoto, CategoryPhotoDto>();

        CreateMap<Address, AddressDto>().ReverseMap();
    }
}
