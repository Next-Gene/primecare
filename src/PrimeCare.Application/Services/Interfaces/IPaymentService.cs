using PrimeCare.Core.Entities;
using PrimeCare.Shared.Dtos.Order;

namespace PrimeCare.Application.Services.Interfaces
{
    public interface IPaymentService
    {
        /// <summary>
        /// Creates or updates a Stripe Payment Intent for the user's cart
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Updated customer cart with payment intent information</returns>
        Task<CustomerCart> CreateOrUpdatePaymentIntent(string userId);

        /// <summary>
        /// Creates a Stripe Checkout Session with shipping address and delivery method
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="checkoutData">Checkout data including shipping address and delivery method</param>
        /// <returns>Stripe Checkout Session URL</returns>
        Task<string> CreateCheckoutSessionAsync(string userId, CheckoutSessionDto checkoutData);

        /// <summary>
        /// Handles Stripe webhook events
        /// </summary>
        /// <param name="json">Webhook payload</param>
        /// <param name="stripeSignature">Stripe signature header</param>
        /// <returns>True if webhook was processed successfully</returns>
        Task<bool> HandleStripeWebhookAsync(string json, string stripeSignature);
    }
}
