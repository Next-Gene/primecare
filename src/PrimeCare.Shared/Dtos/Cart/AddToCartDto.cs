using System.ComponentModel.DataAnnotations;

namespace PrimeCare.Shared.Dtos.Cart
{
    public class AddToCartDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }
}
