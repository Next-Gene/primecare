using Microsoft.AspNetCore.Mvc;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities;
using PrimeCare.Shared.Dtos.Cart;

namespace PrimeCare.Api.Controllers
{
    [ApiController]
    [Route("api/v1/cart")]
    public class CartController : BaseApiController
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        /// <summary>
        /// Get cart by ID
        /// </summary>
        [HttpGet("{cartId}")]
        public async Task<ActionResult<CustomerCart>> GetCartByID(string cartId)
        {
            var cart = await _cartService.GetCartAsync(cartId);
            return Ok(cart ?? new CustomerCart(cartId));
        }

        /// <summary>
        /// Clear cart
        /// </summary>
        [HttpDelete("{cartId}")]
        public async Task<ActionResult<bool>> ClearCart(string cartId)
        {
            var result = await _cartService.ClearCartAsync(cartId);
            return Ok(result);
        }

        /// <summary>
        /// Update entire cart
        /// </summary>
        [HttpPut]
        public async Task<ActionResult<CustomerCart>> UpdateCart([FromBody] CustomerCart cart)
        {
            var updatedCart = await _cartService.UpdateCartAsync(cart);
            return Ok(updatedCart);
        }

        /// <summary>
        /// Add item to cart
        /// </summary>
        [HttpPost("{cartId}/items")]
        public async Task<ActionResult<CustomerCart>> AddItem(string cartId, [FromBody] CartItem item)
        {
            var updatedCart = await _cartService.AddItemAsync(cartId, item);
            return Ok(updatedCart);
        }

        /// <summary>
        /// Remove item from cart
        /// </summary>
        [HttpDelete("{cartId}/items/{productId}")]
        public async Task<ActionResult<CustomerCart>> RemoveItem(string cartId, Guid productId)
        {
            var updatedCart = await _cartService.RemoveItemAsync(cartId, productId);
            return Ok(updatedCart);
        }

        /// <summary>
        /// Update item quantity in cart
        /// </summary>
        [HttpPut("{cartId}/items/{productId}/quantity")]
        public async Task<ActionResult<CustomerCart>> UpdateItemQuantity(string cartId, Guid productId, [FromBody] UpdateQuantityDto dto)
        {
            if (dto.Quantity <= 0)
                return BadRequest("Quantity must be greater than zero.");

            var cart = await _cartService.GetCartAsync(cartId);
            if (cart == null)
                return NotFound($"Cart with ID {cartId} not found.");

            var itemExists = cart.CartItems.Any(i => i.Id == productId);
            if (!itemExists)
                return NotFound($"Item with ID {productId} not found in the cart.");

            var updatedCart = await _cartService.UpdateItemQuantityAsync(cartId, productId, dto.Quantity);
            return Ok(updatedCart);
        }
    }

}
