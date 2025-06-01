using Microsoft.Extensions.Configuration;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities;
using PrimeCare.Core.Entities.Order;
using PrimeCare.Infrastructure.Repositories;
using Stripe;
using Stripe.Checkout;
using PrimeCare.Shared.Dtos.Order;
using AutoMapper;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Product = PrimeCare.Core.Entities.Product;
using PrimeCare.Core.Entities.OrderAggregate;
using Microsoft.Extensions.Logging;

namespace PrimeCare.Application.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(
            IUnitOfWork unitOfWork,
            ICartService cartService,
            IOrderService orderService,
            IConfiguration configuration,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<PaymentService> logger)
        {
            _unitOfWork = unitOfWork;
            _cartService = cartService;
            _orderService = orderService;
            _configuration = configuration;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
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

        public async Task<string> CreateCheckoutSessionAsync(string userId, CheckoutSessionDto checkoutData)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
            var cart = await _cartService.GetCartAsync(userId);

            if (cart == null || !cart.CartItems.Any())
                throw new Exception("Cart is empty");

            cart.DeliveryMethodId = checkoutData.DeliveryMethodId;
            await _cartService.UpdateCartAsync(cart);

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(checkoutData.DeliveryMethodId);
            if (deliveryMethod == null)
                throw new Exception("Invalid delivery method");

            var domain = _configuration["StripeSettings:Domain"];

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                Mode = "payment",
                SuccessUrl = $"{domain}/success?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{domain}/cancel",
                CustomerEmail = checkoutData.ShippingAddress.email, // Add customer email to ensure it's captured
                LineItems = cart.CartItems.Select(item => new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.ProductName,
                            Images = new List<string> { item.PictureUrl }
                        }
                    },
                    Quantity = item.Quantity
                }).ToList(),
                ShippingOptions = new List<SessionShippingOptionOptions>
                {
                    new SessionShippingOptionOptions
                    {
                        ShippingRateData = new SessionShippingOptionShippingRateDataOptions
                        {
                            Type = "fixed_amount",
                            FixedAmount = new SessionShippingOptionShippingRateDataFixedAmountOptions
                            {
                                Amount = (long)(deliveryMethod.Price * 100),
                                Currency = "usd"
                            },
                            DisplayName = deliveryMethod.ShortName,
                            DeliveryEstimate = new SessionShippingOptionShippingRateDataDeliveryEstimateOptions
                            {
                                Minimum = new SessionShippingOptionShippingRateDataDeliveryEstimateMinimumOptions
                                {
                                    Unit = "business_day",
                                    Value = 1
                                },
                                Maximum = new SessionShippingOptionShippingRateDataDeliveryEstimateMaximumOptions
                                {
                                    Unit = "business_day",
                                    Value = 5
                                }
                            }
                        }
                    }
                },
                AllowPromotionCodes = true,
                BillingAddressCollection = "required",
                Metadata = new Dictionary<string, string>
                {
                    {"userId", userId ?? ""},
                    {"deliveryMethodId", checkoutData.DeliveryMethodId.ToString()},
                    {"cartId", cart.Id ?? ""},
                    {"firstName", checkoutData.ShippingAddress.FirstName ?? ""},
                    {"lastName", checkoutData.ShippingAddress.LastName ?? ""},
                    {"street", checkoutData.ShippingAddress.Street ?? ""},
                    {"city", checkoutData.ShippingAddress.City ?? ""},
                    {"state", checkoutData.ShippingAddress.State ?? ""},
                    {"zipCode", checkoutData.ShippingAddress.ZipCode ?? ""},
                    {"phoneNumber", checkoutData.ShippingAddress.phoneNumber ?? ""},
                    {"email", checkoutData.ShippingAddress.email ?? ""} // Add email to metadata as backup
                }
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);
            return session.Url;
        }

        public async Task<bool> HandleStripeWebhookAsync(string json, string stripeSignature)
        {
            var webhookSecret = _configuration["StripeSettings:WebhookSecret"];

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, webhookSecret);

                if (stripeEvent.Type == "checkout.session.completed")
                {
                    var session = stripeEvent.Data.Object as Session;

                    if (session == null)
                    {
                        _logger.LogError("Stripe webhook: Deserialized session is null.");
                        return false;
                    }

                    await ProcessSuccessfulPayment(session);
                }

                return true;
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Stripe exception in webhook handling");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "General exception in webhook handling");
                return false;
            }
        }

        private async Task ProcessSuccessfulPayment(Session session)
        {
            try
            {
                // Validate metadata exists and has required fields
                if (session.Metadata == null)
                {
                    _logger.LogError("Stripe webhook: Session metadata is null");
                    return;
                }

                // Safely extract metadata with null checks
                if (!session.Metadata.TryGetValue("userId", out var userId) || string.IsNullOrEmpty(userId))
                {
                    _logger.LogError("Stripe webhook: userId not found in metadata");
                    return;
                }

                if (!session.Metadata.TryGetValue("deliveryMethodId", out var deliveryMethodIdStr) ||
                    !int.TryParse(deliveryMethodIdStr, out var deliveryMethodId))
                {
                    _logger.LogError("Stripe webhook: Invalid or missing deliveryMethodId in metadata");
                    return;
                }

                // Build shipping address with safe metadata extraction
                var shippingAddress = new PrimeCare.Core.Entities.Order.Address
                {
                    FirstName = GetMetadataValue(session.Metadata, "firstName"),
                    LastName = GetMetadataValue(session.Metadata, "lastName"),
                    Street = GetMetadataValue(session.Metadata, "street"),
                    City = GetMetadataValue(session.Metadata, "city"),
                    State = GetMetadataValue(session.Metadata, "state"),
                    ZipCode = GetMetadataValue(session.Metadata, "zipCode"),
                    PhoneNumber = GetMetadataValue(session.Metadata, "phoneNumber")
                };

                // Get customer email with fallback strategy
                var customerEmail = GetCustomerEmail(session);

                _logger.LogInformation($"Processing payment for user: {userId}, email: {customerEmail}");

                var order = await _orderService.CreateOrderAsync(
                    customerEmail,
                    deliveryMethodId,
                    userId,
                    shippingAddress
                );

                if (order != null)
                {
                    order.PaymentIntentId = session.PaymentIntentId;
                    order.Status = OrderStatus.PaymentReceived;

                    _unitOfWork.Repository<Order>().Update(order);
                    await _unitOfWork.Complete();

                    _logger.LogInformation($"Order {order.Id} successfully processed for payment intent {session.PaymentIntentId}");
                }
                else
                {
                    _logger.LogError("Failed to create order from successful payment session");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing successful payment for session {session.Id}");
                throw; // Re-throw to ensure the webhook returns an error status
            }
        }

        private string GetMetadataValue(IDictionary<string, string> metadata, string key)
        {
            return metadata.TryGetValue(key, out var value) ? value ?? "" : "";
        }

        private string GetCustomerEmail(Session session)
        {
            // Priority order for getting customer email:
            // 1. Session.CustomerEmail (from Stripe checkout)
            // 2. Email from metadata (our backup)
            // 3. Customer details email (if available)
            // 4. Default fallback email

            if (!string.IsNullOrEmpty(session.CustomerEmail))
            {
                return session.CustomerEmail;
            }

            if (session.Metadata != null && session.Metadata.TryGetValue("email", out var metadataEmail) && !string.IsNullOrEmpty(metadataEmail))
            {
                return metadataEmail;
            }

            // If customer details are expanded, try to get email from there
            if (session.CustomerDetails?.Email != null)
            {
                return session.CustomerDetails.Email;
            }

            _logger.LogWarning($"No customer email found for session {session.Id}, using default");
            return "no-reply@primecare.com"; // Use a more appropriate default email for your domain
        }
    }
}