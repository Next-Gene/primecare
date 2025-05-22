
using System.ComponentModel.DataAnnotations;

namespace PrimeCare.Shared.Dtos.Cart
{


    public class CustomerCartDto
    {
        [Required]
        public string Id { get; set; }

        public List<CartItemDto> CartItems { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal TaxAmount { get; set; } // الضريبة

        public decimal TotalPriceWithTax { get; set; } // السعر مع الضريبة
    }


}

