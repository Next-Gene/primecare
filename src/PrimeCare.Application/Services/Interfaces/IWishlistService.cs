using PrimeCare.Core.Entities;

namespace PrimeCare.Application.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for wishlist-related operations such as retrieving, updating, clearing, and modifying wishlist items.
    /// </summary>
    public interface IWishlistService
    {
        /// <summary>
        /// Retrieves a customer's wishlist by its identifier.
        /// </summary>
        /// <param name="wishlistId">The unique identifier of the wishlist.</param>
        /// <returns>The customer's wishlist if found; otherwise, <c>null</c>.</returns>
        Task<CustomerWishlist?> GetWishlistAsync(string wishlistId);

        /// <summary>
        /// Updates the specified customer wishlist.
        /// </summary>
        /// <param name="wishlist">The wishlist to update.</param>
        /// <returns>The updated customer wishlist.</returns>
        Task<CustomerWishlist> UpdateWishlistAsync(CustomerWishlist wishlist);

        /// <summary>
        /// Clears all items from the specified wishlist.
        /// </summary>
        /// <param name="wishlistId">The unique identifier of the wishlist to clear.</param>
        /// <returns><c>true</c> if the wishlist was cleared successfully; otherwise, <c>false</c>.</returns>
        Task<bool> ClearWishlistAsync(string wishlistId);

        /// <summary>
        /// Adds an item to the specified wishlist.
        /// </summary>
        /// <param name="wishlistId">The unique identifier of the wishlist.</param>
        /// <param name="item">The item to add to the wishlist.</param>
        /// <returns>The updated customer wishlist.</returns>
        Task<CustomerWishlist> AddItemAsync(string wishlistId, WishlistItem item);

        /// <summary>
        /// Removes an item from the specified wishlist by its identifier.
        /// </summary>
        /// <param name="wishlistId">The unique identifier of the wishlist.</param>
        /// <param name="itemId">The unique identifier of the item to remove.</param>
        /// <returns>The updated customer wishlist.</returns>
        Task<CustomerWishlist> RemoveItemAsync(string wishlistId, Guid itemId);

        /// <summary>
        /// Checks if an item exists in the specified wishlist.
        /// </summary>
        /// <param name="wishlistId">The unique identifier of the wishlist.</param>
        /// <param name="itemId">The unique identifier of the item to check.</param>
        /// <returns><c>true</c> if the item exists in the wishlist; otherwise, <c>false</c>.</returns>
        Task<bool> ItemExistsAsync(string wishlistId, Guid itemId);
    }
}
