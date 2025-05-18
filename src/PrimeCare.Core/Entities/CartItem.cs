namespace PrimeCare.Core.Entities
{
    /// <summary>
    /// Represents an item in a shopping cart.
    /// </summary>
    public class CartItem
    {
        /// <summary>
        /// Gets or sets the unique identifier for the cart item.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        public string ProductName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the price of the product.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the product in the cart.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the URL of the product's picture.
        /// </summary>
        public string PicrureUrl { get; set; } = null!;

        /// <summary>
        /// Gets or sets the brand of the product.
        /// </summary>
        public string Brand { get; set; } = null!;

        /// <summary>
        /// Gets or sets the category of the product.
        /// </summary>
        public string Category { get; set; } = null!;
    }
}
