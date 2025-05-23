using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities;
using PrimeCare.Core.Entities.Order;
using PrimeCare.Core.Entities.OrderAggregate;
using PrimeCare.Core.Interfaces;
using PrimeCare.Core.Specifications;

namespace PrimeCare.Application.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> _orderRepo;
        private readonly IGenericRepository<DeliveryMethod> _dmRepo;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly ICartService _cartService;

        public OrderService(
            IGenericRepository<Order> orderRepo,
            IGenericRepository<DeliveryMethod> dmRepo,
            IGenericRepository<Product> productRepo,
            ICartService cartService)
        {
            _orderRepo = orderRepo;
            _dmRepo = dmRepo;
            _productRepo = productRepo;
            _cartService = cartService;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string userId, Address shippingAddress)
        {
            var cart = await _cartService.GetCartAsync(userId);
            var items = new List<OrderItem>();

            foreach (var item in cart.CartItems)
            {
                var spec = new ProductWithPhotosSpecification(item.Id);
                var productItem = await _productRepo.GetEntityWithSpecification(spec);

                if (productItem == null)
                    throw new Exception($"Product with ID {item.Id} not found.");

                var mainPhotoUrl = productItem.ProductPhotos?.FirstOrDefault(p => p.IsMain)?.Url ?? string.Empty;
                var itemOrdered = new ProductItemOrderd(productItem.Id, productItem.Name, mainPhotoUrl);
                var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
                items.Add(orderItem);
            }

            var deliveryMethod = await _dmRepo.GetByIdAsync(deliveryMethodId);
            if (deliveryMethod == null)
                throw new Exception("Delivery method not found.");

            var subtotal = items.Sum(i => i.Price * i.Quantity);
            var order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subtotal);

            await _orderRepo.AddAsync(order);
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _dmRepo.GetAllAsync();
        }

        public Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
