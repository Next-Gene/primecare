using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrimeCare.Api.Extensions;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities.Order;
using PrimeCare.Core.Entities.OrderAggregate;
using PrimeCare.Shared.Dtos.Order;
using PrimeCare.Shared.Dtos.User;
using PrimeCare.Shared.Errors;

namespace PrimeCare.Api.Controllers
{

    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = HttpContext.User.RetrieveEmailFromPricipal();
            var address = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);
            var order = await _orderService.CreateOrderAsync(email, orderDto.DeliveryMethodId, userId, address);

            if (order == null) return BadRequest(new ApiResponse(400, "Problem creating order"));

            return Ok(order);
        }



        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var methods = await _orderService.GetDeliveryMethodsAsync();
            return Ok(methods);
        }
    }

}