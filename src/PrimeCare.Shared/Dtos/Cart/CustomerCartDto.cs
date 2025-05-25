
using System.ComponentModel.DataAnnotations;

namespace PrimeCare.Shared.Dtos.Cart
{


    public class CustomerCartDto
    {
        [Required]
        public string Id { get; set; }

        public List<CartItemDto> CartItems { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal TotalPriceWithTax { get; set; }

        public int? DeliveryMethodId { get; set; }

        public string ClientSecret { get; set; } = null!;

        public string PaymentIntentId { get; set; } = null!;


    }
}

