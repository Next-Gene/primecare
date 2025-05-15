namespace PrimeCare.Shared.Dtos.Cart
{
    /// <summary>
    /// Data Transfer Object for updating the quantity of a cart item.
    /// </summary>
    public class UpdateQuantityDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the cart item.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        public string ProductName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the new quantity for the cart item.
        /// </summary>
        public int Quantity { get; set; }
    }
}
