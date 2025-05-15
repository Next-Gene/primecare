namespace PrimeCare.Core.Entities
{
    /// <summary>
    /// Represents a customer's shopping cart containing cart items and total price calculation.
    /// </summary>
    public class CustomerCart
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerCart"/> class.
        /// </summary>
        public CustomerCart()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerCart"/> class with a specified cart identifier.
        /// </summary>
        /// <param name="id">The unique identifier for the cart.</param>
        public CustomerCart(string id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets or sets the unique identifier for the cart.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the list of items in the cart.
        /// </summary>
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();

        /// <summary>
        /// Gets the total price of all items in the cart.
        /// </summary>
        public decimal TotalPrice => CartItems.Sum(item => item.Price * item.Quantity);
    }
}
