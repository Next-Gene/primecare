namespace PrimeCare.Shared.Dtos.Cart
{
    public class UpdateQuantityDto
    {

        public Guid Id { get; set; }
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
    }
}

