namespace PrimeCare.Shared.Dtos.Order
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public string PictureUrl { get; set; }


    }
}
