namespace PrimeCare.Core.Entities
{
    public class CartItem


    {

        public Guid Id { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string PicrureUrl { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public string Category { get; set; } = null!;

    }
}

