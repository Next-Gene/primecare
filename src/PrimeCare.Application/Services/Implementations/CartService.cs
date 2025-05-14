
using System.Text.Json;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities;
using StackExchange.Redis;

namespace PrimeCare.Application.Services.Implementations;

public class CartService : ICartService
{
    private readonly IDatabase _database;
    public CartService(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }

    public async Task<CustomerCart?> GetCartAsync(string cartId)
    {

        var data = await _database.StringGetAsync(cartId);

        return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerCart>(data);
    }

    public async Task<bool> ClearCartAsync(string cartId)
    {
        return await _database.KeyDeleteAsync(cartId);

    }



    public async Task<CustomerCart> UpdateCartAsync(CustomerCart Cart)
    {
        var created = await _database.StringSetAsync(Cart.Id, JsonSerializer.Serialize(Cart), TimeSpan.FromDays(30));

        if (!created) return null!;

        return await GetCartAsync(Cart.Id);
    }


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

    public async Task<CustomerCart> RemoveItemAsync(string cartId, Guid Id)
    {
        var cart = await GetCartAsync(cartId);
        if (cart == null) return null!;

        cart.CartItems.RemoveAll(i => i.Id == Id);

        return await UpdateCartAsync(cart);
    }

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
