using PrimeCare.Core.Entities;

namespace PrimeCare.Application.Services.Interfaces
{

    public interface ICartService
    {
        Task<CustomerCart?> GetCartAsync(string userId);
        Task<CustomerCart> UpdateCartAsync(CustomerCart cart);
        Task<bool> ClearCartAsync(string userId);
        Task<CustomerCart> AddItemAsync(string userId, CartItem item);
        Task<CustomerCart> RemoveItemAsync(string userId, int itemId);
        Task<CustomerCart> UpdateItemQuantityAsync(string userId, int itemId, int quantity);
    }
}
