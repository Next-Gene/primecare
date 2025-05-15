using Microsoft.AspNetCore.Mvc;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities;

namespace PrimeCare.Api.Controllers
{
    /// <summary>
    /// API controller for managing customer wishlists.
    /// </summary>
    [ApiController]
    [Route("api/v1/wishlist")]
    public class WishlistController : BaseApiController
    {
        private readonly IWishlistService _wishlistService;

        /// <summary>
        /// Initializes a new instance of the <see cref="WishlistController"/> class.
        /// </summary>
        /// <param name="wishlistService">The wishlist service for wishlist operations.</param>
        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        /// <summary>
        /// Retrieves a customer's wishlist by its identifier.
        /// </summary>
        /// <param name="wishlistId">The ID of the wishlist.</param>
        /// <returns>The customer's wishlist, or a new wishlist if not found.</returns>
        [HttpGet("{wishlistId}")]
        public async Task<ActionResult<CustomerWishlist>> GetWishlist(string wishlistId)
        {
            var wishlist = await _wishlistService.GetWishlistAsync(wishlistId);
            return Ok(wishlist ?? new CustomerWishlist(wishlistId));
        }

        /// <summary>
        /// Clears all items from the specified wishlist.
        /// </summary>
        /// <param name="wishlistId">The ID of the wishlist to clear.</param>
        /// <returns>True if the operation succeeded; otherwise, false.</returns>
        [HttpDelete("{wishlistId}")]
        public async Task<ActionResult<bool>> ClearWishlist(string wishlistId)
        {
            var result = await _wishlistService.ClearWishlistAsync(wishlistId);
            return Ok(result);
        }

        /// <summary>
        /// Updates the entire wishlist.
        /// </summary>
        /// <param name="wishlist">The updated wishlist object.</param>
        /// <returns>The updated wishlist.</returns>
        [HttpPut]
        public async Task<ActionResult<CustomerWishlist>> UpdateWishlist([FromBody] CustomerWishlist wishlist)
        {
            var updatedWishlist = await _wishlistService.UpdateWishlistAsync(wishlist);
            return Ok(updatedWishlist);
        }

        /// <summary>
        /// Adds an item to the specified wishlist.
        /// </summary>
        /// <param name="wishlistId">The ID of the wishlist.</param>
        /// <param name="item">The item to add.</param>
        /// <returns>The updated wishlist.</returns>
        [HttpPost("{wishlistId}/items")]
        public async Task<ActionResult<CustomerWishlist>> AddItem(string wishlistId, [FromBody] WishlistItem item)
        {
            var updatedWishlist = await _wishlistService.AddItemAsync(wishlistId, item);
            return Ok(updatedWishlist);
        }

        /// <summary>
        /// Removes an item from the specified wishlist.
        /// </summary>
        /// <param name="wishlistId">The ID of the wishlist.</param>
        /// <param name="itemId">The ID of the item to remove.</param>
        /// <returns>The updated wishlist.</returns>
        [HttpDelete("{wishlistId}/items/{itemId}")]
        public async Task<ActionResult<CustomerWishlist>> RemoveItem(string wishlistId, Guid itemId)
        {
            var updatedWishlist = await _wishlistService.RemoveItemAsync(wishlistId, itemId);
            return Ok(updatedWishlist);
        }

        /// <summary>
        /// Checks if an item exists in the specified wishlist.
        /// </summary>
        /// <param name="wishlistId">The ID of the wishlist.</param>
        /// <param name="itemId">The ID of the item to check.</param>
        /// <returns>True if the item exists in the wishlist; otherwise, false.</returns>
        [HttpGet("{wishlistId}/items/{itemId}/exists")]
        public async Task<ActionResult<bool>> ItemExists(string wishlistId, Guid itemId)
        {
            var exists = await _wishlistService.ItemExistsAsync(wishlistId, itemId);
            return Ok(exists);
        }
    }
}
