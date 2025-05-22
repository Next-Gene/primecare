using System.ComponentModel.DataAnnotations;

namespace PrimeCare.Shared.Dtos.Cart
{
    public class AddToWishlistDto
    {
        [Required]
        public int ProductId { get; set; }

    }
}
