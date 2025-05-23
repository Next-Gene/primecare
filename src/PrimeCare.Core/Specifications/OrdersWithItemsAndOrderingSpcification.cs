using PrimeCare.Core.Entities.OrderAggregate;

namespace PrimeCare.Core.Specifications
{
    public class OrdersWithItemsAndOrderingSpcification : BaseSpecification<Order>
    {
        public OrdersWithItemsAndOrderingSpcification(string email) : base(x => x.BuyerEmail == email)
        {
            AddInclude(x => x.OrderItems);
            AddInclude(x => x.DeliveryMethod);
            AddOrderByDescending(x => x.OrderDate);
        }
        public OrdersWithItemsAndOrderingSpcification(int id, string email) : base(o => o.Id == id && o.BuyerEmail == email)
        {
            AddInclude(x => x.OrderItems);
            AddInclude(x => x.DeliveryMethod);
        }
    }
}
