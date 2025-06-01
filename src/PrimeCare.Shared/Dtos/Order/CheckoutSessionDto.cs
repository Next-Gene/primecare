using PrimeCare.Shared.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCare.Shared.Dtos.Order
{
    /// <summary>
    /// DTO for checkout session data including shipping address and delivery method
    /// </summary>
    public class CheckoutSessionDto
    {
        /// <summary>
        /// Customer's shipping address
        /// </summary>
        public AddressDto ShippingAddress { get; set; }

        /// <summary>
        /// Selected delivery method ID
        /// </summary>
        public int DeliveryMethodId { get; set; }

        /// <summary>
        /// Optional: Customer notes for the order
        /// </summary>
        public string? Notes { get; set; }
    }
}
