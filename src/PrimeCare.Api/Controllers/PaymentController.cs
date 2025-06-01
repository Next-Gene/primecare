using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities;
using PrimeCare.Shared.Dtos.Order;

namespace PrimeCare.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaymentController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // CreateOrUpdatePaymentIntent
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CustomerCart>> CreateOrUpdatePaymentIntent()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            var cart = await _paymentService.CreateOrUpdatePaymentIntent(userId);
            if (cart == null)
            {
                return NotFound("Cart not found or payment intent could not be created.");
            }
            return Ok(cart);
        }

        [HttpPost("create-checkout-session")]
        [Authorize]
        public async Task<ActionResult> CreateCheckoutSession([FromBody] CheckoutSessionDto checkoutData)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            // Validate required checkout data
            if (checkoutData?.ShippingAddress == null || checkoutData.DeliveryMethodId <= 0)
            {
                return BadRequest("Shipping address and delivery method are required for checkout.");
            }

            var sessionUrl = await _paymentService.CreateCheckoutSessionAsync(userId, checkoutData);

            return Ok(new { url = sessionUrl });
        }

        [HttpPost("webhook")]
        [AllowAnonymous]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeSignature = Request.Headers["Stripe-Signature"].ToString();

            if (string.IsNullOrEmpty(stripeSignature))
            {
                return BadRequest("Missing Stripe-Signature header.");
            }

            try
            {
                var result = await _paymentService.HandleStripeWebhookAsync(json, stripeSignature);

                if (result)
                    return Ok();
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest($"Webhook error: {ex.Message}");
            }
        }

    }
}