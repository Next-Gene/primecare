using PrimeCare.Core.Entities.Order;
using PrimeCare.Core.Entities.OrderAggregate;

namespace PrimeCare.Application.Services.Interfaces
{
    public interface IOrderService
    {

        Task<Order> CreateOrderAsync(string BuyerEmail, int deliveryMethod, string userId, Address ShippingAddress);
        Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string userId);
        Task<Order> GetOrderByIdAsync(int id, string BuyerEmail);

        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();

    }
}
