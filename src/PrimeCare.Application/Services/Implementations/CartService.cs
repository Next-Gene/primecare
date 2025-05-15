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

    /// <summary>
    /// Retrieves a customer's cart by its identifier.
    /// </summary>
    /// <param name="cartId">The unique identifier of the cart.</param>
    /// <returns>The customer's cart if found; otherwise, <c>null</c>.</returns>
    public async Task<CustomerCart?> GetCartAsync(string cartId)
    {
        var data = await _database.StringGetAsync(cartId);
        return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerCart>(data);
    }

    /// <summary>
    /// Clears all items from the specified cart.
    /// </summary>
    /// <param name="cartId">The unique identifier of the cart to clear.</param>
    /// <returns><c>true</c> if the cart was cleared successfully; otherwise, <c>false</c>.</returns>
    public async Task<bool> ClearCartAsync(string cartId)
    {
        return await _database.KeyDeleteAsync(cartId);
    }

    /// <summary>
    /// Updates the specified customer cart in Redis.
    /// </summary>
    /// <param name="Cart">The cart to update.</param>
    /// <returns>The updated customer cart.</returns>
    public async Task<CustomerCart> UpdateCartAsync(CustomerCart Cart)
    {
        var created = await _database.StringSetAsync(Cart.Id, JsonSerializer.Serialize(Cart), TimeSpan.FromDays(30));
        if (!created) return null!;
        return await GetCartAsync(Cart.Id);
    }

    /// <summary>
    /// Adds an item to the specified cart. If the item already exists, its quantity is increased.
    /// </summary>
    /// <param name="cartId">The unique identifier of the cart.</param>
    /// <param name="newItem">The item to add to the cart.</param>
    /// <returns>The updated customer cart.</returns>
    public async Task<CustomerCart> AddItemAsync(string cartId, CartItem newItem)
    {
        var cart = await GetCartAsync(cartId) ?? new CustomerCart(cartId);
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

    /// <summary>
    /// Removes an item from the specified cart by its identifier.
    /// </summary>
    /// <param name="cartId">The unique identifier of the cart.</param>
    /// <param name="Id">The unique identifier of the item to remove.</param>
    /// <returns>The updated customer cart.</returns>
    public async Task<CustomerCart> RemoveItemAsync(string cartId, Guid Id)
    {
        var cart = await GetCartAsync(cartId);
        if (cart == null) return null!;
        cart.CartItems.RemoveAll(i => i.Id == Id);
        return await UpdateCartAsync(cart);
    }

    /// <summary>
    /// Updates the quantity of a specific item in the cart.
    /// </summary>
    /// <param name="cartId">The unique identifier of the cart.</param>
    /// <param name="Id">The unique identifier of the item to update.</param>
    /// <param name="quantity">The new quantity for the item.</param>
    /// <returns>The updated customer cart.</returns>
    public async Task<CustomerCart> UpdateItemQuantityAsync(string cartId, Guid Id, int quantity)
    {
        var cart = await GetCartAsync(cartId);
        if (cart == null) return null!;
        var item = cart.CartItems.FirstOrDefault(i => i.Id == Id);
        if (item != null)
        {
            item.Quantity = quantity;
        }
        return await UpdateCartAsync(cart);
    }
}
