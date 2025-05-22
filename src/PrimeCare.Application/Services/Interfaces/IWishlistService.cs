using PrimeCare.Core.Entities;

namespace PrimeCare.Application.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for wishlist-related operations such as retrieving, updating, clearing, and modifying wishlist items.
    /// </summary>
    public interface IWishlistService
    {
        Task<CustomerWishlist?> GetWishlistAsync(string userId);
        Task<CustomerWishlist> UpdateWishlistAsync(CustomerWishlist wishlist);
        Task<bool> ClearWishlistAsync(string userId);
        Task<CustomerWishlist> AddItemAsync(string userId, WishlistItem item);
        Task<CustomerWishlist> RemoveItemAsync(string userId, int itemId);
        Task<bool> ItemExistsAsync(string userId, int itemId);
    }
}
