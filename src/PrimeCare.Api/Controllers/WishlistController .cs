using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities;
using PrimeCare.Shared.Dtos.Cart;
using PrimeCare.Shared.Errors;

namespace PrimeCare.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/wishlist")]
    public class WishlistController : BaseApiController
    {
        private readonly IWishlistService _wishlistService;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;

        public WishlistController(IWishlistService wishlistService, IMapper mapper, IProductService productService)
        {
            _wishlistService = wishlistService;
            _mapper = mapper;
            _productService = productService;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;


        [HttpGet]
        public async Task<ActionResult<CustomerWihListDto>> GetWishlist()
        {
            var wishlist = await _wishlistService.GetWishlistAsync(GetUserId()) ?? new CustomerWishlist(GetUserId());
            var dto = _mapper.Map<CustomerWihListDto>(wishlist);
            return Ok(dto);
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> ClearWishlist()
        {
            var result = await _wishlistService.ClearWishlistAsync(GetUserId());
            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<CustomerWishlist>> UpdateWishlist([FromBody] CustomerWishlist wishlist)
        {
            wishlist.Id = GetUserId();
            var updatedWishlist = await _wishlistService.UpdateWishlistAsync(wishlist);
            return Ok(updatedWishlist);
        }
        [HttpPost("items")]
        public async Task<ActionResult<CustomerWishlist>> AddItem([FromBody] AddToWishlistDto dto)
        {
            if (await _wishlistService.ItemExistsAsync(GetUserId(), dto.ProductId))
                return Conflict(new ApiResponse(409, "Item already exists in the wishlist."));

            var product = await _productService.GetByIdAsync(dto.ProductId);
            if (product == null)
                return NotFound(new ApiResponse(404, "Product not found."));

            var item = _mapper.Map<WishlistItem>(product);

            var updatedWishlist = await _wishlistService.AddItemAsync(GetUserId(), item);
            return Ok(updatedWishlist);
        }

        [HttpDelete("items/{productId}")]
        public async Task<ActionResult<CustomerWishlist>> RemoveItem(int productId)
        {
            var updatedWishlist = await _wishlistService.RemoveItemAsync(GetUserId(), productId);
            return Ok(updatedWishlist);
        }

        [HttpGet("items/{productId}/exists")]
        public async Task<ActionResult<bool>> ItemExists(int productId)
        {
            var exists = await _wishlistService.ItemExistsAsync(GetUserId(), productId);
            return Ok(exists);
        }
    }
}
