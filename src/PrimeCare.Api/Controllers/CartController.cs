
using Microsoft.AspNetCore.Mvc;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities;

namespace PrimeCare.Api.Controllers;


public class CartController : BaseApiController
{

    private readonly ICartService _cartService;
    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }
    /// <summary>
    /// Retrieves the current user's cart.
    /// </summary>
    /// <returns>The user's cart or a 404 status if not found.</returns>

    [HttpGet]

    public async Task<ActionResult<CustomerCart>> GetCartByID(string id)
    {
        var cart = await _cartService.GetCartAsync(id);

        return Ok(cart ?? new CustomerCart(id));
    }


    [HttpPost]
    public async Task<ActionResult<CustomerCart>> UpdateCart(CustomerCart cart)
    {
        var updatedCart = await _cartService.UpdateCartAsync(cart);

        return Ok(updatedCart);
    }


    [HttpDelete]

    public async Task<ActionResult<bool>> ClearCart(string id)
    {
        var result = await _cartService.ClearCartAsync(id);
        return Ok(result);
    }
}
