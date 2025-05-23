namespace PrimeCare.Core.Entities.Order
{
    public class ProductItemOrderd
    {
        public ProductItemOrderd()
        {
        }

        public ProductItemOrderd(int productItemId, string productName, string productImageUrl)
        {
            ProductItemId = productItemId;
            ProductName = productName;
            ProductImageUrl = productImageUrl;
        }

        public int ProductItemId { get; set; }

        public string ProductName { get; set; }


        public string ProductImageUrl { get; set; }



    }
}
