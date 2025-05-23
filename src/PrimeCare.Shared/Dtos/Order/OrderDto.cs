using PrimeCare.Shared.Dtos.User;

namespace PrimeCare.Shared.Dtos.Order
{
    public class OrderDto
    {

        public int DeliveryMethodId { get; set; }

        public AddressDto ShippingAddress { get; set; }


    }
}
