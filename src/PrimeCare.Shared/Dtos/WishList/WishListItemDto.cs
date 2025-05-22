using System.ComponentModel.DataAnnotations;

namespace PrimeCare.Shared.Dtos.WishList
{
    public class WishListItemDto
    {

        [Required]
        public int Id { get; set; }

        [Required]
        public string ProductName { get; set; } = null!;

        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required]
        public string PicrureUrl { get; set; } = null!;

        [Required]
        public string Brand { get; set; } = null!;

        [Required]
        public string Category { get; set; } = null!;

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}


