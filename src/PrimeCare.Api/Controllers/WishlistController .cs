using Microsoft.AspNetCore.Mvc;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities;

namespace PrimeCare.Api.Controllers
{
    [ApiController]
    [Route("api/v1/wishlist")]
    public class WishlistController : BaseApiController
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        /// <summary>
        /// Get customer's wishlist by ID
        /// </summary>
        /// <param name="wishlistId">The ID of the wishlist</param>
        /// <returns>Customer's wishlist</returns>
        [HttpGet("{wishlistId}")]
        public async Task<ActionResult<CustomerWishlist>> GetWishlist(string wishlistId)
        {
            var wishlist = await _wishlistService.GetWishlistAsync(wishlistId);
            return Ok(wishlist ?? new CustomerWishlist(wishlistId));
        }

        /// <summary>
        /// Clear all items from the wishlist
        /// </summary>
        /// <param name="wishlistId">The ID of the wishlist to clear</param>
        /// <returns>True if operation succeeded</returns>
        [HttpDelete("{wishlistId}")]
        public async Task<ActionResult<bool>> ClearWishlist(string wishlistId)
        {
            var result = await _wishlistService.ClearWishlistAsync(wishlistId);
            return Ok(result);
        }

        /// <summary>
        /// Update the entire wishlist
        /// </summary>
        /// <param name="wishlist">The updated wishlist object</param>
        /// <returns>The updated wishlist</returns>
        [HttpPut]
        public async Task<ActionResult<CustomerWishlist>> UpdateWishlist([FromBody] CustomerWishlist wishlist)
        {
            var updatedWishlist = await _wishlistService.UpdateWishlistAsync(wishlist);
            return Ok(updatedWishlist);
        }

        /// <summary>
        /// Add an item to the wishlist
        /// </summary>
        /// <param name="wishlistId">The ID of the wishlist</param>
        /// <param name="item">The item to add</param>
        /// <returns>The updated wishlist</returns>
        [HttpPost("{wishlistId}/items")]
        public async Task<ActionResult<CustomerWishlist>> AddItem(string wishlistId, [FromBody] WishlistItem item)
        {
            var updatedWishlist = await _wishlistService.AddItemAsync(wishlistId, item);
            return Ok(updatedWishlist);
        }

        /// <summary>
        /// Remove an item from the wishlist
        /// </summary>
        /// <param name="wishlistId">The ID of the wishlist</param>
        /// <param name="itemId">The ID of the item to remove</param>
        /// <returns>The updated wishlist</returns>
        [HttpDelete("{wishlistId}/items/{itemId}")]
        public async Task<ActionResult<CustomerWishlist>> RemoveItem(string wishlistId, Guid itemId)
        {
            var updatedWishlist = await _wishlistService.RemoveItemAsync(wishlistId, itemId);
            return Ok(updatedWishlist);
        }

        /// <summary>
        /// Check if an item exists in the wishlist
        /// </summary>
        /// <param name="wishlistId">The ID of the wishlist</param>
        /// <param name="itemId">The ID of the item to check</param>
        /// <returns>True if item exists in the wishlist</returns>
        [HttpGet("{wishlistId}/items/{itemId}/exists")]
        public async Task<ActionResult<bool>> ItemExists(string wishlistId, Guid itemId)
        {
            var exists = await _wishlistService.ItemExistsAsync(wishlistId, itemId);
            return Ok(exists);
        }
    }
}