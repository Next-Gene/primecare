namespace PrimeCare.Core.Entities
{
    /// <summary>
    /// Represents an item in a customer's wishlist.
    /// </summary>
    public class WishlistItem
    {
        /// <summary>
        /// Gets or sets the unique identifier for the wishlist item.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        public string ProductName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the price of the product.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the URL of the product's picture.
        /// </summary>
        public string PictureUrl { get; set; } = null!;

        /// <summary>
        /// Gets or sets the brand of the product.
        /// </summary>
        public string Brand { get; set; } = null!;

        /// <summary>
        /// Gets or sets the category of the product.
        /// </summary>
        public string Category { get; set; } = null!;

        /// <summary>
        /// Gets or sets the date and time when the item was added to the wishlist.
        /// </summary>
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
