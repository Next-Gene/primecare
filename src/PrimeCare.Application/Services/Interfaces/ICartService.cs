
using PrimeCare.Core.Entities;

namespace PrimeCare.Application.Services.Interfaces
{
    public interface ICartService
    {

        Task<CustomerCart?> GetCartAsync(string cartId);
        Task<CustomerCart> UpdateCartAsync(CustomerCart Cart);
        Task<bool> ClearCartAsync(string cartId);



        Task<CustomerCart> AddItemAsync(string cartId, CartItem item);
        Task<CustomerCart> RemoveItemAsync(string cartId, Guid Id);
        Task<CustomerCart> UpdateItemQuantityAsync(string cartId, Guid Id, int quantity);
    }
}
