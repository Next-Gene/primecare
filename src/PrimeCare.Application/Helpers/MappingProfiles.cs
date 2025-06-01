using AutoMapper;
using PrimeCare.Core.Entities;
using PrimeCare.Core.Entities.Order;
using PrimeCare.Core.Entities.OrderAggregate;
using PrimeCare.Shared.Dtos.Cart;
using PrimeCare.Shared.Dtos.Categories;
using PrimeCare.Shared.Dtos.Order;
using PrimeCare.Shared.Dtos.Photos;
using PrimeCare.Shared.Dtos.ProductBrand;
using PrimeCare.Shared.Dtos.Products;
using PrimeCare.Shared.Dtos.User;
using PrimeCare.Shared.Dtos.WishList;

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

        CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();
        //CreateMap<CustomerCartDto, CustomerCart>();
        //CreateMap<CartItemDto, CartItem>();
        CreateMap<ProductDto, CartItem>()
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
        .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Name))
        .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => src.ProductPhotos.FirstOrDefault(x => x.IsMain)!.Url))
        .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
        .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.ProductBrand))
        .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));

        CreateMap<ProductDto, WishlistItem>()
       .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
       .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Name))
       .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => src.ProductPhotos.FirstOrDefault(x => x.IsMain)!.Url))
       .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
       .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.ProductBrand))
       .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));
        // CustomerCart ↔ CustomerCartDto

        CreateMap<CustomerCart, CustomerCartDto>()
       .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice))
       .ForMember(dest => dest.TaxAmount, opt => opt.MapFrom(src => src.TaxAmount))
       .ForMember(dest => dest.TotalPriceWithTax, opt => opt.MapFrom(src => src.TotalPriceWithTax));

        CreateMap<CustomerCartDto, CustomerCart>();
        CreateMap<CustomerWishlist, CustomerWihListDto>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        // WishlistItem ↔ WishlistItemDto
        CreateMap<WishlistItem, WishListItemDto>();
        CreateMap<WishListItemDto, WishlistItem>();


        // CartItem ↔ CartItemDto
        CreateMap<CartItem, CartItemDto>();
        CreateMap<CartItemDto, CartItem>();
        CreateMap<AddressDto, PrimeCare.Core.Entities.Order.Address>();
        CreateMap<Order, OrderToReturnDto>()
            .ForMember(dest => dest.DeliveryMethod, opt => opt.MapFrom(src => src.DeliveryMethod.ShortName))
            .ForMember(dest => dest.ShippingPrice, opt => opt.MapFrom(src => src.DeliveryMethod.Price));
        CreateMap<OrderItem, OrderItemDto>().ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ItemOrderd.ProductItemId))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ItemOrderd.ProductName))
            .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => src.ItemOrderd.ProductImageUrl));


    }
}
