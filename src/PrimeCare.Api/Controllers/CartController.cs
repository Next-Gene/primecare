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
    [Route("api/v1/cart")]
    public class CartController : BaseApiController
    {
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;

        public CartController(ICartService cartService, IMapper mapper, IProductService productService)
        {
            _cartService = cartService;
            _mapper = mapper;
            _productService = productService;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpGet]
        public async Task<ActionResult<CustomerCartDto>> GetCart()
        {
            var userId = GetUserId();
            var cart = await _cartService.GetCartAsync(userId);
            if (cart == null)
                return Ok(new CustomerCartDto { Id = userId, CartItems = new List<CartItemDto>() });

            var cartDto = _mapper.Map<CustomerCartDto>(cart);
            return Ok(cartDto);
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> ClearCart()
        {
            var result = await _cartService.ClearCartAsync(GetUserId());
            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<CustomerCartDto>> UpdateCart([FromBody] CustomerCartDto cartDto)
        {
            cartDto.Id = GetUserId();
            var cart = _mapper.Map<CustomerCart>(cartDto);
            var updatedCart = await _cartService.UpdateCartAsync(cart);
            var updatedCartDto = _mapper.Map<CustomerCartDto>(updatedCart);
            return Ok(updatedCartDto);
        }

        [HttpPost("items")]
        public async Task<ActionResult<CustomerCartDto>> AddItem([FromBody] AddToCartDto dto)
        {
            var product = await _productService.GetByIdAsync(dto.ProductId);
            if (product == null)
                return NotFound(new ApiResponse(404, "Product not found."));
            var cartItem = _mapper.Map<CartItem>(product);
            cartItem.Quantity = dto.Quantity;
            cartItem.Id = product.Id;

            var updatedCart = await _cartService.AddItemAsync(GetUserId(), cartItem);
            var updatedCartDto = _mapper.Map<CustomerCartDto>(updatedCart);
            return Ok(updatedCartDto);
        }

        [HttpDelete("items/{productId}")]
        public async Task<ActionResult<CustomerCartDto>> RemoveItem(int productId)
        {
            var updatedCart = await _cartService.RemoveItemAsync(GetUserId(), productId);
            var updatedCartDto = _mapper.Map<CustomerCartDto>(updatedCart);
            return Ok(updatedCartDto);
        }

        [HttpPut("items/{productId}/quantity")]
        public async Task<ActionResult<CustomerCartDto>> UpdateItemQuantity(int productId, [FromBody] UpdateQuantityDto dto)
        {
            if (dto.Quantity <= 0)
                return BadRequest(new ApiResponse(400, "Quantity must be greater than zero."));

            var cart = await _cartService.GetCartAsync(GetUserId());
            if (cart == null)
                return NotFound(new ApiResponse(404, "Cart not found."));

            var itemExists = cart.CartItems.Any(i => i.Id == productId);
            if (!itemExists)
                return NotFound(new ApiResponse(404, $"Item with ID {productId} not found in the cart."));

            var updatedCart = await _cartService.UpdateItemQuantityAsync(GetUserId(), productId, dto.Quantity);
            var updatedCartDto = _mapper.Map<CustomerCartDto>(updatedCart);
            return Ok(updatedCartDto);
        }


    }
}
