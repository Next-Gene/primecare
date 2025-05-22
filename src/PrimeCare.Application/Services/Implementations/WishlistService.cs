using System.Text.Json;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities;
using StackExchange.Redis;

namespace PrimeCare.Application.Services.Implementations
{

    public class WishlistService : IWishlistService
    {
        private readonly IDatabase _database;


        public WishlistService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<CustomerWishlist> AddItemAsync(string userId, WishlistItem item)
        {
            var wishlist = await GetWishlistAsync(userId) ?? new CustomerWishlist(userId);

            if (!wishlist.Items.Any(i => i.Id == item.Id))
            {
                wishlist.Items.Add(item);
                await UpdateWishlistAsync(wishlist);
            }

            return wishlist;
        }

        public async Task<bool> ClearWishlistAsync(string userId)
        {
            return await _database.KeyDeleteAsync(userId);
        }

        public async Task<CustomerWishlist?> GetWishlistAsync(string userId)
        {
            var data = await _database.StringGetAsync(userId);
            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerWishlist>(data);
        }

        public async Task<bool> ItemExistsAsync(string userId, int itemId)
        {
            var wishlist = await GetWishlistAsync(userId);
            return wishlist?.Items.Any(i => i.Id == itemId) ?? false;
        }

        public async Task<CustomerWishlist> RemoveItemAsync(string userId, int itemId)
        {
            var wishlist = await GetWishlistAsync(userId);
            if (wishlist == null) return null!;
            wishlist.Items.RemoveAll(i => i.Id == itemId);
            return await UpdateWishlistAsync(wishlist);
        }

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
