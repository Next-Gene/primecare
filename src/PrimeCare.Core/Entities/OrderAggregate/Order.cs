using PrimeCare.Core.Entities.Order;

namespace PrimeCare.Core.Entities.OrderAggregate
{
    public class Order : BaseEntity
    {
        public Order()
        {
        }

        public Order(IReadOnlyList<OrderItem> orderItems, string buyerEmail, Address shippingAddress, DeliveryMethod deliveryMethod, decimal subtotal)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            OrderItems = orderItems;
            Subtotal = subtotal;
        }

        public string BuyerEmail { get; set; }

        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

        public Address ShippingAddress { get; set; }

        public DeliveryMethod DeliveryMethod { get; set; }

        public IReadOnlyList<OrderItem> OrderItems { get; set; }

        public decimal Subtotal { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public string? PaymentIntentId { get; set; }

        public decimal GetTotal()
        {
            return Subtotal + DeliveryMethod.Price;
        }


    }
}
