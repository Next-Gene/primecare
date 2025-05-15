using System.Text.Json;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities;
using StackExchange.Redis;

namespace PrimeCare.Application.Services.Implementations
{
    /// <summary>
    /// Service for managing customer wishlists using Redis as the backing store.
    /// </summary>
    public class WishlistService : IWishlistService
    {
        private readonly IDatabase _database;

        /// <summary>
        /// Initializes a new instance of the <see cref="WishlistService"/> class.
        /// </summary>
        /// <param name="redis">The Redis connection multiplexer.</param>
        public WishlistService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        /// <summary>
        /// Adds an item to the specified wishlist. If the item already exists, it is not added again.
        /// </summary>
        /// <param name="wishlistId">The unique identifier of the wishlist.</param>
        /// <param name="item">The item to add to the wishlist.</param>
        /// <returns>The updated customer wishlist.</returns>
        public async Task<CustomerWishlist> AddItemAsync(string wishlistId, WishlistItem item)
        {
            var wishlist = await GetWishlistAsync(wishlistId) ?? new CustomerWishlist(wishlistId);

            if (!wishlist.Items.Any(i => i.Id == item.Id))
            {
                wishlist.Items.Add(item);
                await UpdateWishlistAsync(wishlist);
            }

            return wishlist;
        }

        /// <summary>
        /// Clears all items from the specified wishlist.
        /// </summary>
        /// <param name="wishlistId">The unique identifier of the wishlist to clear.</param>
        /// <returns><c>true</c> if the wishlist was cleared successfully; otherwise, <c>false</c>.</returns>
        public async Task<bool> ClearWishlistAsync(string wishlistId)
        {
            return await _database.KeyDeleteAsync(wishlistId);
        }

        /// <summary>
        /// Retrieves a customer's wishlist by its identifier.
        /// </summary>
        /// <param name="wishlistId">The unique identifier of the wishlist.</param>
        /// <returns>The customer's wishlist if found; otherwise, <c>null</c>.</returns>
        public async Task<CustomerWishlist?> GetWishlistAsync(string wishlistId)
        {
            var data = await _database.StringGetAsync(wishlistId);
            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerWishlist>(data);
        }

        /// <summary>
        /// Checks if an item exists in the specified wishlist.
        /// </summary>
        /// <param name="wishlistId">The unique identifier of the wishlist.</param>
        /// <param name="itemId">The unique identifier of the item to check.</param>
        /// <returns><c>true</c> if the item exists in the wishlist; otherwise, <c>false</c>.</returns>
        public async Task<bool> ItemExistsAsync(string wishlistId, Guid itemId)
        {
            var wishlist = await GetWishlistAsync(wishlistId);
            return wishlist?.Items.Any(i => i.Id == itemId) ?? false;
        }

        /// <summary>
        /// Removes an item from the specified wishlist by its identifier.
        /// </summary>
        /// <param name="wishlistId">The unique identifier of the wishlist.</param>
        /// <param name="itemId">The unique identifier of the item to remove.</param>
        /// <returns>The updated customer wishlist.</returns>
        public async Task<CustomerWishlist> RemoveItemAsync(string wishlistId, Guid itemId)
        {
            var wishlist = await GetWishlistAsync(wishlistId);
            if (wishlist == null) return null!;

            wishlist.Items.RemoveAll(i => i.Id == itemId);
            return await UpdateWishlistAsync(wishlist);
        }

        /// <summary>
        /// Updates the specified customer wishlist in Redis.
        /// </summary>
        /// <param name="wishlist">The wishlist to update.</param>
        /// <returns>The updated customer wishlist.</returns>
        public async Task<CustomerWishlist> UpdateWishlistAsync(CustomerWishlist wishlist)
        {
            var created = await _database.StringSetAsync(
                wishlist.Id,
                JsonSerializer.Serialize(wishlist),
                TimeSpan.FromDays(30));

            if (!created) return null!;

            return await GetWishlistAsync(wishlist.Id);
        }
    }
}
