using Microsoft.AspNetCore.Mvc;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities;
using PrimeCare.Shared.Dtos.Cart;

namespace PrimeCare.Api.Controllers
{
    /// <summary>
    /// API controller for managing customer shopping carts.
    /// </summary>
    [ApiController]
    [Route("api/v1/cart")]
    public class CartController : BaseApiController
    {
        private readonly ICartService _cartService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartController"/> class.
        /// </summary>
        /// <param name="cartService">The cart service for cart operations.</param>
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        /// <summary>
        /// Retrieves a cart by its identifier.
        /// </summary>
        /// <param name="cartId">The unique identifier of the cart.</param>
        /// <returns>The customer's cart, or a new cart if not found.</returns>
        [HttpGet("{cartId}")]
        public async Task<ActionResult<CustomerCart>> GetCartByID(string cartId)
        {
            var cart = await _cartService.GetCartAsync(cartId);
            return Ok(cart ?? new CustomerCart(cartId));
        }

        /// <summary>
        /// Clears all items from the specified cart.
        /// </summary>
        /// <param name="cartId">The unique identifier of the cart to clear.</param>
        /// <returns><c>true</c> if the cart was cleared successfully; otherwise, <c>false</c>.</returns>
        [HttpDelete("{cartId}")]
        public async Task<ActionResult<bool>> ClearCart(string cartId)
        {
            var result = await _cartService.ClearCartAsync(cartId);
            return Ok(result);
        }

        /// <summary>
        /// Updates the entire cart.
        /// </summary>
        /// <param name="cart">The cart object with updated information.</param>
        /// <returns>The updated customer cart.</returns>
        [HttpPut]
        public async Task<ActionResult<CustomerCart>> UpdateCart([FromBody] CustomerCart cart)
        {
            var updatedCart = await _cartService.UpdateCartAsync(cart);
            return Ok(updatedCart);
        }

        /// <summary>
        /// Adds an item to the specified cart.
        /// </summary>
        /// <param name="cartId">The unique identifier of the cart.</param>
        /// <param name="item">The item to add to the cart.</param>
        /// <returns>The updated customer cart.</returns>
        [HttpPost("{cartId}/items")]
        public async Task<ActionResult<CustomerCart>> AddItem(string cartId, [FromBody] CartItem item)
        {
            var updatedCart = await _cartService.AddItemAsync(cartId, item);
            return Ok(updatedCart);
        }

        /// <summary>
        /// Removes an item from the specified cart.
        /// </summary>
        /// <param name="cartId">The unique identifier of the cart.</param>
        /// <param name="productId">The unique identifier of the product to remove.</param>
        /// <returns>The updated customer cart.</returns>
        [HttpDelete("{cartId}/items/{productId}")]
        public async Task<ActionResult<CustomerCart>> RemoveItem(string cartId, Guid productId)
        {
            var updatedCart = await _cartService.RemoveItemAsync(cartId, productId);
            return Ok(updatedCart);
        }

        /// <summary>
        /// Updates the quantity of a specific item in the cart.
        /// </summary>
        /// <param name="cartId">The unique identifier of the cart.</param>
        /// <param name="productId">The unique identifier of the product to update.</param>
        /// <param name="dto">The DTO containing the new quantity for the item.</param>
        /// <returns>The updated customer cart, or an error if the cart or item is not found.</returns>
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
