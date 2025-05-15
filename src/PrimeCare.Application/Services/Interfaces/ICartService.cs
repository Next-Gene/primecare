using PrimeCare.Core.Entities;

namespace PrimeCare.Application.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for cart-related operations such as retrieving, updating, clearing, and modifying cart items.
    /// </summary>
    public interface ICartService
    {
        /// <summary>
        /// Retrieves a customer's cart by its identifier.
        /// </summary>
        /// <param name="cartId">The unique identifier of the cart.</param>
        /// <returns>The customer's cart if found; otherwise, <c>null</c>.</returns>
        Task<CustomerCart?> GetCartAsync(string cartId);

        /// <summary>
        /// Updates the specified customer cart.
        /// </summary>
        /// <param name="Cart">The cart to update.</param>
        /// <returns>The updated customer cart.</returns>
        Task<CustomerCart> UpdateCartAsync(CustomerCart Cart);

        /// <summary>
        /// Clears all items from the specified cart.
        /// </summary>
        /// <param name="cartId">The unique identifier of the cart to clear.</param>
        /// <returns><c>true</c> if the cart was cleared successfully; otherwise, <c>false</c>.</returns>
        Task<bool> ClearCartAsync(string cartId);

        /// <summary>
        /// Adds an item to the specified cart.
        /// </summary>
        /// <param name="cartId">The unique identifier of the cart.</param>
        /// <param name="item">The item to add to the cart.</param>
        /// <returns>The updated customer cart.</returns>
        Task<CustomerCart> AddItemAsync(string cartId, CartItem item);

        /// <summary>
        /// Removes an item from the specified cart by its identifier.
        /// </summary>
        /// <param name="cartId">The unique identifier of the cart.</param>
        /// <param name="Id">The unique identifier of the item to remove.</param>
        /// <returns>The updated customer cart.</returns>
        Task<CustomerCart> RemoveItemAsync(string cartId, Guid Id);

        /// <summary>
        /// Updates the quantity of a specific item in the cart.
        /// </summary>
        /// <param name="cartId">The unique identifier of the cart.</param>
        /// <param name="Id">The unique identifier of the item to update.</param>
        /// <param name="quantity">The new quantity for the item.</param>
        /// <returns>The updated customer cart.</returns>
        Task<CustomerCart> UpdateItemQuantityAsync(string cartId, Guid Id, int quantity);
    }
}
