using Microsoft.AspNetCore.Mvc;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities;

namespace PrimeCare.Api.Controllers
{
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        /// <summary>
        /// Retrieves the cart by user ID.
        /// </summary>
        [HttpGet("api/getcart/{id}", Name = "GetCartById")]
        public async Task<ActionResult<CustomerCart>> GetCartByID(string id)
        {
            var cart = await _cartService.GetCartAsync(id);
            return Ok(cart ?? new CustomerCart(id));
        }

        /// <summary>
        /// Updates the user's cart.
        /// </summary>
        [HttpPost("api/updatecart", Name = "UpdateCart")]
        public async Task<ActionResult<CustomerCart>> UpdateCart([FromBody] CustomerCart cart)
        {
            var updatedCart = await _cartService.UpdateCartAsync(cart);
            return Ok(updatedCart);
        }

        /// <summary>
        /// Clears the cart for a specific user ID.
        /// </summary>
        [HttpDelete("api/clearcart/{id}", Name = "ClearCart")]
        public async Task<ActionResult<bool>> ClearCart(string id)
        {
            var result = await _cartService.ClearCartAsync(id);
            return Ok(result);
        }
    }
}
