namespace PrimeCare.Core.Entities
{
    /// <summary>
    /// Represents a customer's wishlist containing wishlist items.
    /// </summary>
    public class CustomerWishlist
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerWishlist"/> class.
        /// </summary>
        public CustomerWishlist()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerWishlist"/> class with a specified wishlist identifier.
        /// </summary>
        /// <param name="id">The unique identifier for the wishlist.</param>
        public CustomerWishlist(string id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets or sets the unique identifier for the wishlist.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the list of items in the wishlist.
        /// </summary>
        public List<WishlistItem> Items { get; set; } = new List<WishlistItem>();

        /// <summary>
        /// Gets the total number of items in the wishlist.
        /// </summary>
        public int TotalItems => Items.Count;
    }
}
