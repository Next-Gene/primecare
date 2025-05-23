namespace PrimeCare.Core.Entities.Order
{

    public class DeliveryMethod : BaseEntity
    {
        public string ShortName { get; set; } = null!;
        public string DeliveryTime { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
    }



}

