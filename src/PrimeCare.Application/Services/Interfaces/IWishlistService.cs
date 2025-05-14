using PrimeCare.Core.Entities;

namespace PrimeCare.Application.Services.Interfaces
{
    public interface IWishlistService
    {
        Task<CustomerWishlist?> GetWishlistAsync(string wishlistId);
        Task<CustomerWishlist> UpdateWishlistAsync(CustomerWishlist wishlist);
        Task<bool> ClearWishlistAsync(string wishlistId);

        Task<CustomerWishlist> AddItemAsync(string wishlistId, WishlistItem item);
        Task<CustomerWishlist> RemoveItemAsync(string wishlistId, Guid itemId);
        Task<bool> ItemExistsAsync(string wishlistId, Guid itemId);
    }
}
