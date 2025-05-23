using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities;
using PrimeCare.Core.Entities.Order;
using PrimeCare.Core.Entities.OrderAggregate;
using PrimeCare.Core.Specifications;
using PrimeCare.Infrastructure.Repositories;

namespace PrimeCare.Application.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly ICartService _cartService;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(ICartService cartService, IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
            _cartService = cartService;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string userId, Address shippingAddress)
        {
            var cart = await _cartService.GetCartAsync(userId);
            var items = new List<OrderItem>();

            foreach (var item in cart.CartItems)
            {
                var spec = new ProductWithPhotosSpecification(item.Id);
                var productItem = await _unitOfWork.Repository<Product>().GetEntityWithSpecification(spec);

                if (productItem == null)
                    throw new Exception($"Product with ID {item.Id} not found.");

                var mainPhotoUrl = productItem.ProductPhotos?.FirstOrDefault(p => p.IsMain)?.Url ?? string.Empty;
                var itemOrdered = new ProductItemOrderd(productItem.Id, productItem.Name, mainPhotoUrl);
                var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
                items.Add(orderItem);
            }

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            if (deliveryMethod == null)
                throw new Exception("Delivery method not found.");

            var subtotal = items.Sum(i => i.Price * i.Quantity);
            var order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subtotal);

            _unitOfWork.Repository<Order>().Add(order);
            var result = await _unitOfWork.Complete();

            if (result <= 0) return null;

            // Clear the cart after order creation
            await _cartService.ClearCartAsync(userId);
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpcification(id, buyerEmail);
            return await _unitOfWork.Repository<Order>().GetEntityWithSpecification(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpcification(buyerEmail);
            return await _unitOfWork.Repository<Order>().GetAllWithSpecificationAsync(spec);
        }
    }
}
