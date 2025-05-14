namespace PrimeCare.Core.Entities
{
    public class CustomerCart
    {




        public CustomerCart()
        {

        }

        public CustomerCart(string id)
        {

            Id = id;

        }
        public string Id { get; set; }
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public decimal TotalPrice => CartItems.Sum(item => item.Price * item.Quantity);

    }
}
