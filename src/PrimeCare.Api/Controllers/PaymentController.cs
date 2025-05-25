using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities;

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
        public async Task<ActionResult> CreateCheckoutSession()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var sessionUrl = await _paymentService.CreateCheckoutSessionAsync(userId);

            return Ok(new { url = sessionUrl });
        }


    }
}
