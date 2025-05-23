namespace PrimeCare.Core.Entities.Order
{
    public class OrderItem : BaseEntity
    {
        public OrderItem()
        {
        }

        public OrderItem(ProductItemOrderd itemOrderd, decimal price, int quantity)
        {
            ItemOrderd = itemOrderd;
            Price = price;
            Quantity = quantity;
        }

        public ProductItemOrderd ItemOrderd { get; set; }

        public decimal Price { get; set; }
        public int Quantity { get; set; }



    }
}
