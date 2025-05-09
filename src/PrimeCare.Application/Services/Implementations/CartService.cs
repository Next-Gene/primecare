
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
}
