using System.Text.Json;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities;
using StackExchange.Redis;

namespace PrimeCare.Application.Services.Implementations;

/// <summary>
/// Service for managing customer shopping carts using Redis as the backing store.
/// </summary>
public class CartService : ICartService
{
    private readonly IDatabase _database;

    /// <summary>
    /// Initializes a new instance of the <see cref="CartService"/> class.
    /// </summary>
    /// <param name="redis">The Redis connection multiplexer.</param>
    public CartService(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }

    public async Task<CustomerCart?> GetCartAsync(string userId)
    {
        var data = await _database.StringGetAsync(userId);
        return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerCart>(data);
    }

    public async Task<bool> ClearCartAsync(string userId)
    {
        return await _database.KeyDeleteAsync(userId);
    }

    public async Task<CustomerCart> UpdateCartAsync(CustomerCart cart)
    {
        var created = await _database.StringSetAsync(cart.Id, JsonSerializer.Serialize(cart), TimeSpan.FromDays(30));
        if (!created) return null!;
        return await GetCartAsync(cart.Id);
    }

    public async Task<CustomerCart> AddItemAsync(string userId, CartItem newItem)
    {
        var cart = await GetCartAsync(userId) ?? new CustomerCart(userId);
        var existingItem = cart.CartItems.FirstOrDefault(i => i.Id == newItem.Id);
        if (existingItem != null)
        {
            existingItem.Quantity += newItem.Quantity;
        }
        else
        {
            cart.CartItems.Add(newItem);
        }
        return await UpdateCartAsync(cart);
    }

    public async Task<CustomerCart> RemoveItemAsync(string userId, int itemId)
    {
        var cart = await GetCartAsync(userId);
        if (cart == null) return null!;
        cart.CartItems.RemoveAll(i => i.Id == itemId);
        return await UpdateCartAsync(cart);
    }
    public async Task<CustomerCart> UpdateItemQuantityAsync(string userId, int itemId, int quantity)
    {
        var cart = await GetCartAsync(userId);
        if (cart == null) return null!;
        var item = cart.CartItems.FirstOrDefault(i => i.Id == itemId);
        if (item != null)
        {
            if (quantity <= 0)
            {
                cart.CartItems.Remove(item);
            }
            else
            {
                item.Quantity = quantity;
            }
        }
        return await UpdateCartAsync(cart);
    }
}
