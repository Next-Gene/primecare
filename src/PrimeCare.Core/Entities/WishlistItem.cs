namespace PrimeCare.Core.Entities
{
    public class WishlistItem
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public string PicrureUrl { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public string Category { get; set; } = null!;
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
