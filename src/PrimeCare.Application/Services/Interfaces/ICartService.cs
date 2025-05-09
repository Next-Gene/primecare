
using PrimeCare.Core.Entities;

namespace PrimeCare.Application.Services.Interfaces
{
    public interface ICartService
    {

        Task<CustomerCart?> GetCartAsync(string cartId);
        Task<CustomerCart> UpdateCartAsync(CustomerCart Cart);
        Task<bool> ClearCartAsync(string cartId);

    }
}
