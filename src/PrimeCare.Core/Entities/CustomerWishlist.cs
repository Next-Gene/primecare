namespace PrimeCare.Core.Entities
{
    public class CustomerWishlist
    {
        public CustomerWishlist()
        {
        }

        public CustomerWishlist(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
        public List<WishlistItem> Items { get; set; } = new List<WishlistItem>();
        public int TotalItems => Items.Count;
    }

}


