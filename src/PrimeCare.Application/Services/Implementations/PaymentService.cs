using Microsoft.Extensions.Configuration;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities;
using PrimeCare.Core.Entities.Order;
using PrimeCare.Infrastructure.Repositories;
using Stripe;
using Stripe.Checkout;
using Product = PrimeCare.Core.Entities.Product;

namespace PrimeCare.Application.Services.Implementations
{
    public class PaymentService : IPaymentService

    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartService _cartService;
        private readonly IConfiguration _configuration;
        public PaymentService(IUnitOfWork unitOfWork, ICartService cartService, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _cartService = cartService;
            _configuration = configuration;
        }

        public async Task<CustomerCart> CreateOrUpdatePaymentIntent(string userId)
        {

            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
            var cart = await _cartService.GetCartAsync(userId);
            var ShippingPrice = 0m;
            if (cart.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync((int)cart.DeliveryMethodId.Value);

                ShippingPrice = deliveryMethod.Price;

            }

            foreach (var item in cart.CartItems)
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                if (item.Price != product.Price)
                {
                    item.Price = product.Price;
                }
            }
            var Service = new PaymentIntentService();

            PaymentIntent Intent;

            if (string.IsNullOrEmpty(cart.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)((cart.CartItems.Sum(i => i.Quantity * (i.Price * 100)) + (long)ShippingPrice * 100)),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" },

                };
                Intent = await Service.CreateAsync(options);
                cart.PaymentIntentId = Intent.Id;
                cart.ClientSecret = Intent.ClientSecret;

            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)((cart.CartItems.Sum(i => i.Quantity * (i.Price * 100)) + (long)ShippingPrice * 100)),

                };
                await Service.UpdateAsync(cart.PaymentIntentId, options);
            }
            await _cartService.UpdateCartAsync(cart);
            return cart;
        }



        public async Task<string> CreateCheckoutSessionAsync(string userId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
            var cart = await _cartService.GetCartAsync(userId);
            if (cart == null || !cart.CartItems.Any())
                throw new Exception("Cart is empty");

            var domain = _configuration["StripeSettings:Domain"]; // e.g. https://yourfrontend.com

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                Mode = "payment",
                SuccessUrl = $"{domain}/success",
                CancelUrl = $"{domain}/cancel",
                LineItems = cart.CartItems.Select(item => new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.ProductName
                        }
                    },
                    Quantity = item.Quantity
                }).ToList()
            };

            var service = new SessionService();
            Session session = await service.CreateAsync(options);

            return session.Url;
        }

    }
}
